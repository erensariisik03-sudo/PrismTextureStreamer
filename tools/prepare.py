#!/usr/bin/env python3
"""Unity 4.7 -> Unity 6 gecis hazirligi. Kullanim: python3 tools/prepare.py ExportedProject
Idempotent: birden fazla calistirilabilir."""
import os, re, shutil, sys

root = sys.argv[1] if len(sys.argv) > 1 else "ExportedProject"
here = os.path.dirname(os.path.abspath(__file__))
assets = os.path.join(root, "Assets")
stats = {}

def bump(k, n=1): stats[k] = stats.get(k, 0) + n

# 1) Cakisan / gereksiz DLL'ler (Unity 6 kendi sagliyor)
for dll in ["System.Runtime.CompilerServices.Unsafe.dll", "System.Runtime.InteropServices.dll",
            "NativeToolkitWP8.dll", "VungleSDKProxy.dll"]:
    for f in (os.path.join(assets, "Plugins", dll), os.path.join(assets, "Plugins", dll + ".meta")):
        if os.path.exists(f):
            os.remove(f); bump("silinen_dll")

# 2) Eski ProjectSettings dosyalarini at (Unity 6 taze uretir; ayarlar BuildScript'te)
ps = os.path.join(root, "ProjectSettings")
for f in ["ProjectSettings.asset", "ProjectVersion.txt", "GraphicsSettings.asset", "NetworkManager.asset",
          "MasterServerInterface.asset", "NavMeshLayers.asset", "Physics2DSettings.asset",
          "DelayedCallManager.asset", "QualitySettings.asset"]:
    p = os.path.join(ps, f)
    if os.path.exists(p):
        os.remove(p); bump("silinen_ayar")

# 3) Paketler
os.makedirs(os.path.join(root, "Packages"), exist_ok=True)
with open(os.path.join(root, "Packages", "manifest.json"), "w") as f:
    f.write('''{
  "dependencies": {
    "com.unity.ugui": "2.0.0",
    "com.unity.modules.ai": "1.0.0",
    "com.unity.modules.androidjni": "1.0.0",
    "com.unity.modules.animation": "1.0.0",
    "com.unity.modules.assetbundle": "1.0.0",
    "com.unity.modules.audio": "1.0.0",
    "com.unity.modules.cloth": "1.0.0",
    "com.unity.modules.director": "1.0.0",
    "com.unity.modules.imageconversion": "1.0.0",
    "com.unity.modules.imgui": "1.0.0",
    "com.unity.modules.jsonserialize": "1.0.0",
    "com.unity.modules.particlesystem": "1.0.0",
    "com.unity.modules.physics": "1.0.0",
    "com.unity.modules.physics2d": "1.0.0",
    "com.unity.modules.screencapture": "1.0.0",
    "com.unity.modules.terrain": "1.0.0",
    "com.unity.modules.terrainphysics": "1.0.0",
    "com.unity.modules.ui": "1.0.0",
    "com.unity.modules.unitywebrequest": "1.0.0",
    "com.unity.modules.unitywebrequesttexture": "1.0.0",
    "com.unity.modules.unitywebrequestwww": "1.0.0",
    "com.unity.modules.vehicles": "1.0.0",
    "com.unity.modules.video": "1.0.0"
  }
}
''')

# 4) Editor build script + reklamsiz stub'lar
os.makedirs(os.path.join(assets, "Editor"), exist_ok=True)
shutil.copy(os.path.join(here, "BuildScript.cs"), os.path.join(assets, "Editor", "BuildScript.cs"))
stub_names = {"Ads.cs", "Admob_Universal.cs", "Heyzap_Controller.cs"}
overlay = os.path.join(here, "overlay")
for dp, _, fns in os.walk(assets):
    for fn in fns:
        if fn in stub_names and fn in os.listdir(overlay):
            shutil.copy(os.path.join(overlay, fn), os.path.join(dp, fn)); bump("stub")  # .meta (GUID) korunur

# 5) Kaynak kod donusumleri
def find_close(s, i):
    d = 0
    for j in range(i, len(s)):
        if s[j] == "(": d += 1
        elif s[j] == ")":
            d -= 1
            if d == 0: return j
    return -1

SM = "UnityEngine.SceneManagement.SceneManager"
ADD = "UnityEngine.SceneManagement.LoadSceneMode.Additive"
CALLS = [  # uzun isimler once
    ("Application.LoadLevelAdditiveAsync(", lambda a: f"{SM}.LoadSceneAsync({a}, {ADD})"),
    ("Application.LoadLevelAdditive(",      lambda a: f"{SM}.LoadScene({a}, {ADD})"),
    ("Application.LoadLevelAsync(",         lambda a: f"{SM}.LoadSceneAsync({a})"),
    ("Application.LoadLevel(",              lambda a: f"{SM}.LoadScene({a})"),
]

def fix_calls(s):
    for token, fmt in CALLS:
        while True:
            i = s.find(token)
            if i < 0: break
            op = i + len(token) - 1
            cl = find_close(s, op)
            if cl < 0: break
            s = s[:i] + fmt(s[op+1:cl]) + s[cl+1:]
            bump("loadlevel")
    return s

COMP = {"rigidbody": "Rigidbody", "renderer": "Renderer", "audio": "AudioSource", "camera": "Camera",
        "light": "Light", "collider": "Collider", "animation": "Animation", "hingeJoint": "HingeJoint"}
# yalniz guvenli on ekler (RaycastHit/Collision.collider|rigidbody gecerli oldugu icin dokunma)
PREFIX = r"(?:base|this|gameObject|transform)"
comp_re = re.compile(r"\b(" + PREFIX + r")\.(" + "|".join(COMP) + r")\b(?!\s*\()")

def fix_src(s):
    o = s
    s = fix_calls(s)
    s = s.replace("Application.loadedLevelName", f"{SM}.GetActiveScene().name")
    s = re.sub(r"Application\.loadedLevel\b", f"{SM}.GetActiveScene().buildIndex", s)
    s = comp_re.sub(lambda m: f"{m.group(1)}.GetComponent<{COMP[m.group(2)]}>()", s)
    if s != o: bump("degisen_dosya")
    return s

for dp, _, fns in os.walk(assets):
    if os.sep + "Editor" in dp and dp.endswith("Editor"): continue
    for fn in fns:
        if not fn.endswith(".cs") or fn == "BuildScript.cs": continue
        p = os.path.join(dp, fn)
        try:
            s = open(p, encoding="utf-8-sig").read()
        except Exception:
            continue
        n = fix_src(s)
        if n != s:
            open(p, "w", encoding="utf-8").write(n)

print("prepare.py tamam:", stats)

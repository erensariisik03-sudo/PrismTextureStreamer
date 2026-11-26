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

# 6) Ilk build'in derleme hatalari icin duzeltmeler
# 6a) Cift AssemblyInfo.cs (Unity kendisi uretiyor)
for dp, _, fns in os.walk(assets):
    if os.path.basename(dp) == "Properties":
        for fn in fns:
            if fn.startswith("AssemblyInfo.cs"):
                os.remove(os.path.join(dp, fn)); bump("assemblyinfo")

# 6b) Unity 4'teki UnityEngine.RPC attribute'u Unity 5+'ta yok: kendi tanimimiz
rpc = os.path.join(assets, "Plugins", "Assembly-CSharp-firstpass", "RPC.cs")
os.makedirs(os.path.dirname(rpc), exist_ok=True)
open(rpc, "w").write("using System;\n\n[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]\npublic class RPC : Attribute\n{\n}\n")

# 6c) UnityScript/Boo calisma zamani bagimliliklarini kaynaktan cikar
def edit(rel_glob_name, pairs):
    for dp, _, fns in os.walk(assets):
        if rel_glob_name in fns:
            p = os.path.join(dp, rel_glob_name)
            t = open(p, encoding="utf-8-sig").read()
            o = t
            for a, b in pairs:
                t = t.replace(a, b)
            if t != o:
                open(p, "w", encoding="utf-8").write(t); bump("elle_duzeltme")

edit("DepthOfFieldScatter.cs", [
    ("using Boo.Lang.Runtime;\n", ""),
    ("RuntimeServices.EqualityOperator(cbDrawArgs, null)", "(cbDrawArgs == null)"),
    ("RuntimeServices.EqualityOperator(cbPoints, null)", "(cbPoints == null)")])
edit("Tonemapping.cs", [
    ("using UnityScript.Lang;\n", ""),
    ("Extensions.get_length((System.Array)remapCurve.keys)", "remapCurve.keys.Length")])
edit("CharacterMotor.cs", [("using Boo.Lang;\n", "")])

# 6d) ILogger belirsizligi (Uniject.ILogger vs UnityEngine.ILogger)
for dp, _, fns in os.walk(assets):
    for fn in fns:
        if not fn.endswith(".cs"): continue
        p = os.path.join(dp, fn)
        try: t = open(p, encoding="utf-8-sig").read()
        except Exception: continue
        if "using Uniject;" in t and "using UnityEngine;" in t and re.search(r"\bILogger\b", t) \
                and "using ILogger =" not in t:
            t = t.replace("using Uniject;", "using Uniject;\nusing ILogger = Uniject.ILogger;", 1)
            open(p, "w", encoding="utf-8").write(t); bump("ilogger")

# 7) Ikinci dalga: Unity 6'da olmayan eski API'ler
# 7a) iOS Xcode yardimcilari (UnityEditor.* ad alani, oyuncu kodunda gereksiz)
xc = os.path.join(assets, "Plugins", "Assembly-CSharp-firstpass", "UnityEditor")
if os.path.isdir(xc):
    shutil.rmtree(xc); bump("xcode_klasoru")
    if os.path.exists(xc + ".meta"): os.remove(xc + ".meta")

# 7b) Eski GUITexture (kaldirildi) icin bos uyumluluk sinifi: ayna dokusu alanlari derlensin
gt = os.path.join(assets, "Plugins", "Assembly-CSharp-firstpass", "GUITexture.cs")
open(gt, "w").write("using UnityEngine;\n\n// Unity 6'da GUITexture yok; yalnizca derleme icin.\npublic class GUITexture : MonoBehaviour\n{\n\tpublic Texture texture;\n\tpublic Color color = Color.white;\n\tpublic Rect pixelInset;\n}\n")

# 7c) Genel regex duzeltmeleri
GEN = [
    (re.compile(r"\bScreen\.showCursor\s*=\s*([^;]+);"), r"Cursor.visible = \1;"),
    (re.compile(r"\bScreen\.lockCursor\s*=\s*true;"), "Cursor.lockState = CursorLockMode.Locked;"),
    (re.compile(r"\bScreen\.lockCursor\s*=\s*false;"), "Cursor.lockState = CursorLockMode.None;"),
    (re.compile(r"\.castShadows\s*=\s*true;"), ".shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;"),
    (re.compile(r"\.castShadows\s*=\s*false;"), ".shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;"),
    (re.compile(r"Application\.RegisterLogCallback\(([A-Za-z_][\w.]*)\)"), r"Application.logMessageReceived += \1"),
]
for dp, _, fns in os.walk(assets):
    if os.path.basename(dp) == "Editor": continue
    for fn in fns:
        if not fn.endswith(".cs") or fn == "BuildScript.cs": continue
        p = os.path.join(dp, fn)
        try: t = open(p, encoding="utf-8-sig").read()
        except Exception: continue
        o = t
        for rx, rep in GEN:
            t = rx.sub(rep, t)
        if t != o:
            open(p, "w", encoding="utf-8").write(t); bump("genel_duzeltme")

print("prepare.py tamam:", stats)

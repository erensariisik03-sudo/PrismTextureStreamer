# PrismTextureStreamer – Türkçe Sürüm (Kurulum)

## Derleme (Windows)
1. Visual Studio 2022 kur ("C++ ile masaüstü geliştirme" iş yükü seçili olsun).
2. `PrismTextureStreamerFB.sln` dosyasını aç.
3. Üstte **Release** ve **x64** seç → Build > Build Solution.
4. Çıkan `PrismTextureStreamerFB.dll` dosyasını şuraya kopyala:
   `...\Euro Truck Simulator 2\bin\win_x64\plugins\`
   (`plugins` klasörü yoksa oluştur.)

## Kullanım
- Oyunda **Ctrl+F8** ile menüyü aç/kapat.
- GPS / Gösterge Paneli / Özel ekran ekle → kaynak pencereyi seç → **Değişiklikleri Uygula**.

## Notlar
- Sadece tek kişilik modda kullan. TruckersMP gibi çok oyunculu servisler oyun belleğini
  değiştiren eklentileri yasaklayabilir.
- Oyun güncellenince "pattern scan" adresleri bozulabilir; o zaman eklenti çalışmaz.
- Menü fontu `C:\Windows\Fonts\Arial.ttf` dosyasından yüklenir (ğ ı ş İ için ayar eklendi).

## GitHub ile derleme (Visual Studio'suz)
1. Bu dosyaların hepsini (`.github` klasörü dahil) kendi GitHub deponun köküne yükle.
   `.github` klasörü yüklenmezse: Add file > Create new file > ad olarak
   `.github/workflows/build.yml` yaz ve içeriği yapıştır.
2. Depoda **Actions** sekmesine gir; derleme push ile otomatik başlar
   (başlamazsa soldan "Derle (Windows x64)" > Run workflow).
3. Yeşil tik çıkınca çalışmanın içinde **Artifacts** bölümünden
   `PrismTextureStreamerFB-TR` dosyasını indir, zip'ten `PrismTextureStreamerFB.dll` çıkar.
4. DLL'yi oyunun `bin\win_x64\plugins\` klasörüne at.

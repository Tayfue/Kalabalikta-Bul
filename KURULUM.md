# KALABALIKTA BUL

**Kural:** Aynı renkteki topların arasında rengi biraz daha açık olan tek topu bul ve dokun. Her doğru top 1 puan; yanlış topa dokunursan süreden 2 saniye gider.
**Kazanma / kaybetme:** `if (kalanSure <= 0f)` oyun biter → `if (skor >= hedefSkor)` kazandın, değilse kaybettin.
**Kontrol:** Fare ya da parmakla dokunma.

## Ekranlar
1. **Splash** – oyunun adı, 2,5 sn sonra ya da dokununca menü.
2. **Menü** – Oyna / Ayarlar (ses aç-kapat, oyun süresi 20 / 30 / 45 sn).
3. **Oyun** – toplar, skor, süre, sonuç paneli (Tekrar Oyna / Menü).

## Unity'de kurulum
1. **Window → TextMeshPro → Import TMP Essential Resources**.
2. Üst menü **Kalabalıkta Bul → Her Şeyi Kur**.
3. Game penceresi **1080x1920 Portrait**, Splash sahnesinde **Play**.

## Dosyalar
| Klasör | Ne var |
|---|---|
| Assets/Scripts | TopYonetici (oyunun beyni), Splash, MenuYonetici |
| Assets/DersBetikleri | Merhaba, Hareket, Sekme, TopUretici (B aşaması deneyleri işlenmiş) |
| Assets/Editor | Sahneleri kuran araç (oyunun parçası değil, Claude ile yazıldı) |

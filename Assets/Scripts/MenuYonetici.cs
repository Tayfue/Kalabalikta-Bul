using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// 2. ekran: ana menü (Oyna / Ayarlar)
// Ayarlar: ses açık-kapalı, oyun süresi (Kısa 20 sn / Normal 30 sn / Uzun 45 sn)
// Seçimler PlayerPrefs ile kaydedilir; Oyun sahnesi açılınca oradan okunur.
public class MenuYonetici : MonoBehaviour
{
    [SerializeField] GameObject anaPanel;
    [SerializeField] GameObject ayarlarPaneli;
    [SerializeField] TMP_Text sesYazisi;       // "Ses: Açık"
    [SerializeField] TMP_Text sureYazisi;      // "Süre: Normal (30 sn)"
    [SerializeField] AudioSource hoparlor;
    [SerializeField] AudioClip tikSesi;

    void Start()
    {
        anaPanel.SetActive(true);
        ayarlarPaneli.SetActive(false);
        YazilariGuncelle();
    }

    public void Oyna()
    {
        Tikla();
        SceneManager.LoadScene("Oyun");
    }

    public void AyarlariAc()
    {
        Tikla();
        anaPanel.SetActive(false);
        ayarlarPaneli.SetActive(true);
    }

    public void GeriDon()
    {
        Tikla();
        anaPanel.SetActive(true);
        ayarlarPaneli.SetActive(false);
    }

    public void SesDegistir()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) PlayerPrefs.SetInt("ses", 0);
        else PlayerPrefs.SetInt("ses", 1);
        Tikla();
        YazilariGuncelle();
    }

    // 0 = Kısa, 1 = Normal, 2 = Uzun
    public void SureSec(int sure)
    {
        PlayerPrefs.SetInt("sure", sure);
        Tikla();
        YazilariGuncelle();
    }

    void YazilariGuncelle()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) sesYazisi.text = "Ses: Açık";
        else sesYazisi.text = "Ses: Kapalı";

        switch (PlayerPrefs.GetInt("sure", 1))
        {
            case 0: sureYazisi.text = "Süre: Kısa (20 sn)"; break;
            case 1: sureYazisi.text = "Süre: Normal (30 sn)"; break;
            default: sureYazisi.text = "Süre: Uzun (45 sn)"; break;
        }
    }

    void Tikla()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) hoparlor.PlayOneShot(tikSesi);
    }
}

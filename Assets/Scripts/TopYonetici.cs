using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class TopYonetici : MonoBehaviour
{
    [Header("Oyun ayarları")]
    [SerializeField] GameObject topPrefab;
    [SerializeField] int ilkAdet = 8;
    [SerializeField] int artis = 2;
    [SerializeField] int enFazlaAdet = 40;
    [SerializeField] float renkFarki = 0.4f;
    [SerializeField] float cezaSure = 2f;
    [SerializeField] int hedefSkor = 10;
    [SerializeField] float aralik = 0.8f;

    [Header("Bağlantılar")]
    [SerializeField] TMP_Text bilgiYazisi;
    [SerializeField] TMP_Text rekorYazisi;
    [SerializeField] GameObject sonucPaneli;
    [SerializeField] TMP_Text sonucYazisi;

    [Header("Sesler")]
    [SerializeField] AudioSource hoparlor;
    [SerializeField] AudioClip dogruSesi;
    [SerializeField] AudioClip yanlisSesi;
    [SerializeField] AudioClip kazanmaSesi;
    [SerializeField] AudioClip kaybetmeSesi;

    GameObject[] toplar = new GameObject[0];
    int farkliNo;
    int skor = 0;
    int rekor;
    int sureSecimi;
    float kalanSure;
    bool oyunSuruyor = false;
    Vector2 sinir;

    void Start()
    {
        sonucPaneli.SetActive(false);

        sureSecimi = PlayerPrefs.GetInt("sure", 1);
        switch (sureSecimi)
        {
            case 0: kalanSure = 20f; break;
            case 1: kalanSure = 30f; break;
            default: kalanSure = 45f; break;
        }

        rekor = PlayerPrefs.GetInt("rekor" + sureSecimi, 0);
        rekorYazisi.text = "Rekor: " + rekor;

        Camera k = Camera.main;
        sinir = new Vector2(k.orthographicSize * k.aspect - 0.6f, k.orthographicSize - 1.5f);

        oyunSuruyor = true;
        YeniTur();
    }

    void YeniTur()
    {
        for (int i = 0; i < toplar.Length; i++)
            Destroy(toplar[i]);

        int adet = Mathf.Min(enFazlaAdet, ilkAdet + skor * artis);
        toplar = new GameObject[adet];

        Color anaRenk;
        switch (Random.Range(0, 4))
        {
            case 0: anaRenk = new Color(0.9f, 0.25f, 0.25f); break;
            case 1: anaRenk = new Color(0.2f, 0.45f, 0.9f); break;
            case 2: anaRenk = new Color(0.25f, 0.75f, 0.35f); break;
            default: anaRenk = new Color(0.65f, 0.35f, 0.85f); break;
        }
        Color farkliRenk = Color.Lerp(anaRenk, Color.white, renkFarki);

        farkliNo = Random.Range(0, adet);

        for (int i = 0; i < adet; i++)
        {
            toplar[i] = Instantiate(topPrefab, BosYerBul(i), Quaternion.identity);
            if (i == farkliNo) toplar[i].GetComponent<SpriteRenderer>().color = farkliRenk;
            else toplar[i].GetComponent<SpriteRenderer>().color = anaRenk;
        }
    }

    Vector3 BosYerBul(int kacTopVar)
    {
        Vector3 yer = Vector3.zero;
        for (int deneme = 0; deneme < 30; deneme++)
        {
            yer = new Vector3(Random.Range(-sinir.x, sinir.x), Random.Range(-sinir.y, sinir.y - 1f), 0f);

            bool bos = true;
            for (int i = 0; i < kacTopVar; i++)
            {
                if (Vector3.Distance(yer, toplar[i].transform.position) < aralik) bos = false;
            }
            if (bos) return yer;
        }
        return yer;
    }

    void Update()
    {
        if (!oyunSuruyor) return;

        kalanSure = kalanSure - Time.deltaTime;
        bilgiYazisi.text = "Skor: " + skor + "   Süre: " + kalanSure.ToString("0.0");

        if (kalanSure <= 0f)
        {
            Bitir();
            return;
        }

        var isaret = Pointer.current;
        if (isaret == null || !isaret.press.wasPressedThisFrame) return;
        Vector2 dunya = Camera.main.ScreenToWorldPoint(isaret.position.ReadValue());

        for (int i = 0; i < toplar.Length; i++)
        {
            if (toplar[i].GetComponent<Collider2D>().OverlapPoint(dunya))
            {
                if (i == farkliNo)
                {
                    skor = skor + 1;
                    renkFarki = Mathf.Max(0.15f, renkFarki - 0.02f);
                    SesCal(dogruSesi);
                    YeniTur();
                }
                else
                {
                    kalanSure = kalanSure - cezaSure;
                    SesCal(yanlisSesi);
                    Debug.Log("Yanlış top! " + cezaSure + " saniye gitti.");
                }
                return;
            }
        }
    }

    void Bitir()
    {
        oyunSuruyor = false;
        kalanSure = 0f;
        bilgiYazisi.text = "Skor: " + skor + "   Süre: 0.0";
        sonucPaneli.SetActive(true);

        if (skor > rekor)
        {
            rekor = skor;
            PlayerPrefs.SetInt("rekor" + sureSecimi, rekor);
        }
        rekorYazisi.text = "Rekor: " + rekor;

        if (skor >= hedefSkor)
        {
            sonucYazisi.text = "KAZANDIN!\nSkor: " + skor + "\nRekor: " + rekor;
            SesCal(kazanmaSesi);
        }
        else
        {
            sonucYazisi.text = "SÜRE BİTTİ!\nSkor: " + skor + " / " + hedefSkor + "\nRekor: " + rekor;
            SesCal(kaybetmeSesi);
        }
        Debug.Log(sonucYazisi.text);
    }

    public void TekrarOyna()
    {
        SceneManager.LoadScene("Oyun");
    }

    public void MenuyeDon()
    {
        SceneManager.LoadScene("Menu");
    }

    void SesCal(AudioClip klip)
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) hoparlor.PlayOneShot(klip);
    }
}

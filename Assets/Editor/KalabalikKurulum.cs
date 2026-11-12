using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using TMPro;

public static class KalabalikKurulum
{
    const string SAHNE = "Assets/Scenes/";
    static readonly Color ARKA = new Color(0.12f, 0.14f, 0.22f);
    static readonly Vector2 ORTA = new Vector2(0.5f, 0.5f);
    static Sprite kare, daire;
    static Sprite butonSprite;

    [MenuItem("Kalabalıkta Bul/Her Şeyi Kur")]
    static void HerSeyiKur()
    {
        if (Resources.Load<TMP_Settings>("TMP Settings") == null)
        {
            EditorUtility.DisplayDialog("Önce TextMeshPro",
                "Önce Window > TextMeshPro > Import TMP Essential Resources ile içe aktar, sonra bu menüyü tekrar çalıştır.", "Tamam");
            return;
        }
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        System.IO.Directory.CreateDirectory("Assets/Scenes");
        System.IO.Directory.CreateDirectory("Assets/Prefabs");
        AssetDatabase.Refresh();

        kare = SpriteHazirla("Assets/Sprites/Kare.png", 64, false);
        daire = SpriteHazirla("Assets/Sprites/Daire.png", 256, false);
        SpriteHazirla("Assets/Sprites/Buton.png", 100, false);
        var butonImp = (TextureImporter)AssetImporter.GetAtPath("Assets/Sprites/Buton.png");
        butonImp.spriteBorder = new Vector4(48, 48, 48, 48);
        butonImp.SaveAndReimport();
        butonSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Buton.png");

        SplashKur();
        MenuKur();
        OyunKur();
        DeneyMerhabaKur();
        DeneyHareketKur();
        DeneySekmeKur();

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(SAHNE + "Splash.unity", true),
            new EditorBuildSettingsScene(SAHNE + "Menu.unity", true),
            new EditorBuildSettingsScene(SAHNE + "Oyun.unity", true),
        };
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        AssetDatabase.SaveAssets();
        EditorSceneManager.OpenScene(SAHNE + "Splash.unity");
        EditorUtility.DisplayDialog("Hazır!",
            "Sahneler kuruldu. Splash sahnesi açık, Play'e bas.\nGame penceresinde çözünürlüğü 1080x1920 Portrait yap.", "Tamam");
    }

    static void SplashKur()
    {
        var s = YeniSahne(5f, ARKA);
        var c = CanvasKur();
        Color[] renkler = { new Color(0.2f, 0.45f, 0.9f), new Color(0.2f, 0.45f, 0.9f), new Color(0.55f, 0.72f, 0.97f), new Color(0.2f, 0.45f, 0.9f) };
        for (int i = 0; i < 4; i++)
            Resim("Top" + i, c, daire, renkler[i], ORTA, new Vector2(-330 + i * 220, 150), new Vector2(170, 170));
        Yazi("Baslik", c, "KALABALIKTA\nBUL", 120, ORTA, new Vector2(0, 550), new Vector2(1000, 350), Color.white);
        Yazi("AltYazi", c, "dokun ve başla", 55, ORTA, new Vector2(0, -350), new Vector2(900, 100), Color.white);
        new GameObject("Splash").AddComponent<Splash>();
        EditorSceneManager.SaveScene(s, SAHNE + "Splash.unity");
    }

    static void MenuKur()
    {
        var s = YeniSahne(5f, ARKA);
        var c = CanvasKur();

        var ana = Panel("AnaPanel", c, new Color(0, 0, 0, 0));
        Yazi("Baslik", ana.transform, "KALABALIKTA\nBUL", 110, ORTA, new Vector2(0, 600), new Vector2(1000, 330), Color.white);
        var oyna = Buton("Oyna", ana.transform, "Oyna", new Vector2(0, 0));
        var ayarlar = Buton("Ayarlar", ana.transform, "Ayarlar", new Vector2(0, -200));

        var ayarP = Panel("AyarlarPaneli", c, new Color(0, 0, 0, 0.6f));
        Yazi("Baslik", ayarP.transform, "AYARLAR", 110, ORTA, new Vector2(0, 650), new Vector2(1000, 200), Color.white);
        var sesYazi = Yazi("SesYazisi", ayarP.transform, "Ses: Açık", 70, ORTA, new Vector2(0, 420), new Vector2(1000, 120), Color.white);
        var sesBtn = Buton("SesButonu", ayarP.transform, "Aç / Kapat", new Vector2(0, 270));
        var sureYazi = Yazi("SureYazisi", ayarP.transform, "Süre: Normal (30 sn)", 70, ORTA, new Vector2(0, 60), new Vector2(1000, 120), Color.white);
        var kisa = Buton("Kisa", ayarP.transform, "Kısa", new Vector2(-330, -90), new Vector2(300, 140));
        var normal = Buton("Normal", ayarP.transform, "Normal", new Vector2(0, -90), new Vector2(300, 140));
        var uzun = Buton("Uzun", ayarP.transform, "Uzun", new Vector2(330, -90), new Vector2(300, 140));
        var geri = Buton("Geri", ayarP.transform, "Geri", new Vector2(0, -500));

        var mg = new GameObject("MenuYonetici");
        var hoparlor = mg.AddComponent<AudioSource>();
        hoparlor.playOnAwake = false;
        var m = mg.AddComponent<MenuYonetici>();
        Bagla(m, "anaPanel", ana);
        Bagla(m, "ayarlarPaneli", ayarP);
        Bagla(m, "sesYazisi", sesYazi);
        Bagla(m, "sureYazisi", sureYazi);
        Bagla(m, "hoparlor", hoparlor);
        Bagla(m, "tikSesi", Ses("Tik"));

        UnityEventTools.AddPersistentListener(oyna.onClick, m.Oyna);
        UnityEventTools.AddPersistentListener(ayarlar.onClick, m.AyarlariAc);
        UnityEventTools.AddPersistentListener(geri.onClick, m.GeriDon);
        UnityEventTools.AddPersistentListener(sesBtn.onClick, m.SesDegistir);
        UnityEventTools.AddIntPersistentListener(kisa.onClick, m.SureSec, 0);
        UnityEventTools.AddIntPersistentListener(normal.onClick, m.SureSec, 1);
        UnityEventTools.AddIntPersistentListener(uzun.onClick, m.SureSec, 2);

        EditorSceneManager.SaveScene(s, SAHNE + "Menu.unity");
    }

    static void OyunKur()
    {
        var s = YeniSahne(6f, ARKA);

        var tg = new GameObject("Top");
        tg.AddComponent<SpriteRenderer>().sprite = daire;
        tg.AddComponent<CircleCollider2D>().radius = 0.5f;
        tg.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        var topPrefab = PrefabUtility.SaveAsPrefabAsset(tg, "Assets/Prefabs/Top.prefab");
        Object.DestroyImmediate(tg);

        var c = CanvasKur();
        var bilgi = Yazi("BilgiYazisi", c, "Skor: 0   Süre: 30.0", 60, new Vector2(0.5f, 1f), new Vector2(0, -100), new Vector2(1000, 120), Color.white);
        var rekorYazi = Yazi("RekorYazisi", c, "Rekor: 0", 50, new Vector2(0.5f, 0f), new Vector2(0, 90), new Vector2(800, 100), Color.white);

        var sonuc = Panel("SonucPaneli", c, new Color(0, 0, 0, 0.75f));
        var sonucYazi = Yazi("SonucYazisi", sonuc.transform, "SÜRE BİTTİ!", 95, ORTA, new Vector2(0, 280), new Vector2(1000, 450), Color.white);
        var tekrar = Buton("Tekrar", sonuc.transform, "Tekrar Oyna", new Vector2(0, -100));
        var menu = Buton("Menu", sonuc.transform, "Menü", new Vector2(0, -300));

        var yg = new GameObject("TopYonetici");
        var hoparlor = yg.AddComponent<AudioSource>();
        hoparlor.playOnAwake = false;
        var y = yg.AddComponent<TopYonetici>();
        Bagla(y, "topPrefab", topPrefab);
        Bagla(y, "bilgiYazisi", bilgi);
        Bagla(y, "rekorYazisi", rekorYazi);
        Bagla(y, "sonucPaneli", sonuc);
        Bagla(y, "sonucYazisi", sonucYazi);
        Bagla(y, "hoparlor", hoparlor);
        Bagla(y, "dogruSesi", Ses("Dogru"));
        Bagla(y, "yanlisSesi", Ses("Yanlis"));
        Bagla(y, "kazanmaSesi", Ses("Kazandin"));
        Bagla(y, "kaybetmeSesi", Ses("Kaybettin"));

        UnityEventTools.AddPersistentListener(tekrar.onClick, y.TekrarOyna);
        UnityEventTools.AddPersistentListener(menu.onClick, y.MenuyeDon);

        EditorSceneManager.SaveScene(s, SAHNE + "Oyun.unity");
    }

    static void DeneyMerhabaKur()
    {
        var s = YeniSahne(5f, new Color(0.2f, 0.2f, 0.25f));
        var g = new GameObject("Kare");
        g.AddComponent<SpriteRenderer>().sprite = kare;
        g.AddComponent<BoxCollider2D>().size = Vector2.one;
        g.transform.localScale = new Vector3(2f, 2f, 1f);
        g.AddComponent<Merhaba>();
        EditorSceneManager.SaveScene(s, SAHNE + "Deney_Merhaba.unity");
    }

    static void DeneyHareketKur()
    {
        var s = YeniSahne(5f, new Color(0.2f, 0.2f, 0.25f));
        var g = new GameObject("Pierrot");
        g.AddComponent<SpriteRenderer>().sprite = kare;
        g.AddComponent<Hareket>();
        EditorSceneManager.SaveScene(s, SAHNE + "Deney_Hareket.unity");
    }

    static void DeneySekmeKur()
    {
        var s = YeniSahne(5f, new Color(0.2f, 0.2f, 0.25f));
        var tg = new GameObject("Top");
        tg.AddComponent<SpriteRenderer>().sprite = daire;
        tg.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        tg.AddComponent<Sekme>();
        var topPrefab = PrefabUtility.SaveAsPrefabAsset(tg, "Assets/Prefabs/DeneyTop.prefab");
        Object.DestroyImmediate(tg);

        var u = new GameObject("TopUretici").AddComponent<TopUretici>();
        Bagla(u, "topPrefab", topPrefab.GetComponent<Sekme>());
        EditorSceneManager.SaveScene(s, SAHNE + "Deney_Sekme.unity");
    }

    static Sprite SpriteHazirla(string yol, int ppu, bool altPivot)
    {
        var imp = (TextureImporter)AssetImporter.GetAtPath(yol);
        imp.textureType = TextureImporterType.Sprite;
        imp.spriteImportMode = SpriteImportMode.Single;
        imp.spritePixelsPerUnit = ppu;
        var ayar = new TextureImporterSettings();
        imp.ReadTextureSettings(ayar);
        ayar.spriteAlignment = altPivot ? (int)SpriteAlignment.BottomCenter : (int)SpriteAlignment.Center;
        imp.SetTextureSettings(ayar);
        imp.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(yol);
    }

    static Scene YeniSahne(float boyut, Color arka)
    {
        var s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var kg = new GameObject("Main Camera");
        kg.tag = "MainCamera";
        var k = kg.AddComponent<Camera>();
        k.orthographic = true;
        k.orthographicSize = boyut;
        k.clearFlags = CameraClearFlags.SolidColor;
        k.backgroundColor = arka;
        kg.AddComponent<AudioListener>();
        kg.transform.position = new Vector3(0, 0, -10);
        return s;
    }

    static Transform CanvasKur()
    {
        var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var sc = go.GetComponent<CanvasScaler>();
        sc.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        sc.referenceResolution = new Vector2(1080, 1920);
        sc.matchWidthOrHeight = 0.5f;
        new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        return go.transform;
    }

    static RectTransform UIObje(string ad, Transform ebeveyn, Vector2 anchor, Vector2 konum, Vector2 boyut)
    {
        var go = new GameObject(ad, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(ebeveyn, false);
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = ORTA;
        rt.anchoredPosition = konum;
        rt.sizeDelta = boyut;
        return rt;
    }

    static TextMeshProUGUI Yazi(string ad, Transform eb, string metin, float punto, Vector2 anchor, Vector2 konum, Vector2 boyut, Color renk)
    {
        var t = UIObje(ad, eb, anchor, konum, boyut).gameObject.AddComponent<TextMeshProUGUI>();
        t.text = metin;
        t.fontSize = punto;
        t.alignment = TextAlignmentOptions.Center;
        t.color = renk;
        t.raycastTarget = false;
        return t;
    }

    static Image Resim(string ad, Transform eb, Sprite sp, Color renk, Vector2 anchor, Vector2 konum, Vector2 boyut)
    {
        var img = UIObje(ad, eb, anchor, konum, boyut).gameObject.AddComponent<Image>();
        img.sprite = sp;
        img.color = renk;
        img.preserveAspect = true;
        return img;
    }

    static Button Buton(string ad, Transform eb, string metin, Vector2 konum)
    {
        return Buton(ad, eb, metin, konum, new Vector2(600, 140));
    }

    static Button Buton(string ad, Transform eb, string metin, Vector2 konum, Vector2 boyut)
    {
        var img = Resim(ad, eb, butonSprite, new Color(1f, 0.95f, 0.85f), ORTA, konum, boyut);
        img.preserveAspect = false;
        img.type = Image.Type.Sliced;
        var b = img.gameObject.AddComponent<Button>();
        Yazi("Yazi", img.transform, metin, 60, ORTA, Vector2.zero, boyut, Color.black);
        return b;
    }

    static GameObject Panel(string ad, Transform eb, Color renk)
    {
        var rt = UIObje(ad, eb, ORTA, Vector2.zero, Vector2.zero);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.gameObject.AddComponent<Image>().color = renk;
        return rt.gameObject;
    }

    static AudioClip Ses(string ad)
    {
        return AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sesler/" + ad + ".wav");
    }

    static void BaglaDizi(Object hedef, string alan, Object[] degerler)
    {
        var so = new SerializedObject(hedef);
        var p = so.FindProperty(alan);
        if (p == null)
        {
            Debug.LogError(hedef.name + " içinde '" + alan + "' alanı bulunamadı");
            return;
        }
        p.arraySize = degerler.Length;
        for (int i = 0; i < degerler.Length; i++)
            p.GetArrayElementAtIndex(i).objectReferenceValue = degerler[i];
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void Bagla(Object hedef, string alan, Object deger)
    {
        var so = new SerializedObject(hedef);
        var p = so.FindProperty(alan);
        if (p == null)
        {
            Debug.LogError(hedef.name + " içinde '" + alan + "' alanı bulunamadı");
            return;
        }
        p.objectReferenceValue = deger;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}

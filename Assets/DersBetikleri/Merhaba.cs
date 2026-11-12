using UnityEngine;
using UnityEngine.InputSystem;

public class Merhaba : MonoBehaviour
{
    int sayac = 0;

    void Start()
    {
        Debug.Log("Merhaba! Ben " + gameObject.name);
    }

    void Update()
    {
        var isaret = Pointer.current;
        if (isaret == null || !isaret.press.wasPressedThisFrame) return;

        Vector2 ekran = isaret.position.ReadValue();
        Vector2 dunya = Camera.main.ScreenToWorldPoint(ekran);

        if (GetComponent<Collider2D>().OverlapPoint(dunya))
        {
            sayac = sayac + 2;
            Debug.Log("Dokunuldu: " + sayac);

            transform.localScale = transform.localScale * 0.8f;
        }
    }
}

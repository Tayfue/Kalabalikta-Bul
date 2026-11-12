using UnityEngine;

public class TopUretici : MonoBehaviour
{
    [SerializeField] Sekme topPrefab;
    [SerializeField] int adet = 1;

    void Start()
    {
        for (int i = 0; i < adet; i++)
        {
            Sekme t = Instantiate(topPrefab, Vector3.zero, Quaternion.identity);
            t.hiz = Random.insideUnitCircle.normalized * Random.Range(2f, 5f);

            var sr = t.GetComponent<SpriteRenderer>();
            switch (i % 3)
            {
                case 0: sr.color = Color.red; break;
                case 1: sr.color = Color.blue; break;
                default: sr.color = Color.green; break;
            }
        }
    }
}

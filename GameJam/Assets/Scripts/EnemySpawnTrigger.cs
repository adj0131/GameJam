using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToActivate;

    void Start()
    {
        foreach (GameObject enemy in enemiesToActivate)
        {
            if (enemy != null)
                enemy.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        foreach (GameObject enemy in enemiesToActivate)
        {
            if (enemy != null)
                enemy.SetActive(true);
        }

        Destroy(gameObject);
    }
}
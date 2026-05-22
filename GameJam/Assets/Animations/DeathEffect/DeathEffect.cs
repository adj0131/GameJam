using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public float lifetime = 1f;

    void Start()
    {
        // automatically destroy after animation finishes
        Destroy(gameObject, lifetime);
    }
}

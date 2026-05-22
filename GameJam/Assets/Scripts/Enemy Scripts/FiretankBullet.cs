using UnityEngine;

public class FiretankBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = -transform.right * speed;

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return;

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Floor") ||
            hitInfo.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
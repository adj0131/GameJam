using UnityEngine;

public class FireTankBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Called by FireTankEnemy immediately after Instantiate
    public void Init(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Start()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return;

        // Don't hit the enemy that fired this bullet
        if (hitInfo.GetComponent<FireTankEnemy>() != null) return;

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    // Safety net: destroy bullet after 5 seconds if it hits nothing
    private void OnEnable()
    {
        Invoke(nameof(DestroySelf), 5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}

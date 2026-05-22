using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 15;
    public float slowDuration = 5f;
    public float slowAmount = 0.5f;         // 0.5 = 50% slower

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Start()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return;
        if (hitInfo.GetComponent<FridgeEnemy>() != null) return;

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);

            // Search in parent and children too
            PlayerMovement movement = hitInfo.GetComponentInParent<PlayerMovement>();
            if (movement == null) movement = hitInfo.GetComponentInChildren<PlayerMovement>();
            if (movement != null)
                movement.ApplySlow(slowAmount, slowDuration);

            IcedEffect iced = hitInfo.GetComponentInParent<IcedEffect>();
            if (iced == null) iced = hitInfo.GetComponentInChildren<IcedEffect>();
            if (iced != null)
                iced.ApplyIce(slowDuration);

            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        Invoke(nameof(DestroySelf), 5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
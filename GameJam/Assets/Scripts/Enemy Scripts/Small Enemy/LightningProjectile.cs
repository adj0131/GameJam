using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float lifetime = 4f;
    [SerializeField] int damage = 1;

    private Vector2 direction;
    private bool isMoving = false;

    void Start()
    {
        Invoke("StartMoving", 1f);
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void StartMoving()
    {
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return;
        if (hitInfo.GetComponent<RoboEnemy>() != null) return;

        PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (hitInfo.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
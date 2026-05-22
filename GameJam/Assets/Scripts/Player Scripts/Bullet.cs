using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rb;

    void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
{
    if (hitInfo == null) return;

    FireTankEnemy fireTank = hitInfo.GetComponent<FireTankEnemy>();
    if (fireTank != null && fireTank.gameObject != null)
    {
        fireTank.TakeDamage(damage);
        Destroy(gameObject);
        return;
    }
}
}

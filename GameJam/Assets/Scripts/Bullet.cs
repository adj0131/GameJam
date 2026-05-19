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
        if (hitInfo)
        {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {            
            enemy.TakeDamage(damage);
        }
         Destroy(gameObject);
    }
    }

}
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
        if (fireTank != null)
        {
            fireTank.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        FridgeEnemy fridge = hitInfo.GetComponent<FridgeEnemy>();
        if (fridge != null)
        {
            fridge.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        RoboEnemy robo = hitInfo.GetComponent<RoboEnemy>();
        if (robo != null)
        {
            robo.TakeHit();
            Destroy(gameObject);
            return;
        }
        ChainsawEnemy chainsaw = hitInfo.GetComponent<ChainsawEnemy>();
        if (chainsaw != null)        {
            chainsaw.TakeHit();
            Destroy(gameObject);
            return; 
        }
    }

    void OnEnable()
    {
        Invoke(nameof(DestroySelf), 5f);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rb;

    [Header("Ice Upgrade")]
    public bool isIce = false;          // set by PlayerMovement if ice upgrade is active
    public float slowAmount = 0.5f;     // 50% slower
    public float slowDuration = 3f;

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
            ApplyIceToEnemy(hitInfo.gameObject);
            Destroy(gameObject);
            return;
        }

        FridgeEnemy fridge = hitInfo.GetComponent<FridgeEnemy>();
        if (fridge != null)
        {
            fridge.TakeDamage(damage);
            ApplyIceToEnemy(hitInfo.gameObject);
            Destroy(gameObject);
            return;
        }

        RoboEnemy robo = hitInfo.GetComponent<RoboEnemy>();
        if (robo != null)
        {
            robo.TakeHit();
            ApplyIceToEnemy(hitInfo.gameObject);
            Destroy(gameObject);
            return;
        }

        ChainsawEnemy chainsaw = hitInfo.GetComponent<ChainsawEnemy>();
        if (chainsaw != null)
        {
            chainsaw.TakeHit();
            ApplyIceToEnemy(hitInfo.gameObject);
            Destroy(gameObject);
            return;
        }
    }

    // Applies slow + blue tint to the enemy if ice upgrade is active.
    // Enemies need an IcedEffect component attached in the Inspector for the tint.
    void ApplyIceToEnemy(GameObject enemy)
    {
        if (!isIce) return;

        IcedEffect iced = enemy.GetComponent<IcedEffect>();
        if (iced == null) iced = enemy.GetComponentInChildren<IcedEffect>();
        if (iced != null) iced.ApplyIce(slowDuration);
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
using UnityEngine;

public class ChainsawDamage : MonoBehaviour
{
    public int damage = 20;
    public float damageCooldown = 0.5f;
    private float damageTimer = 0f;

    private void Update()
    {
        damageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return;
        if (hitInfo.GetComponent<Bullet>() != null) return;

        PlayerHealth player = hitInfo.GetComponentInParent<PlayerHealth>();
        if (player != null && damageTimer <= 0f)
        {
            player.TakeDamage(damage);
            damageTimer = damageCooldown;
        }
    }

    private void OnTriggerStay2D(Collider2D hitInfo)
    {
        if (hitInfo == null) return;
        if (hitInfo.GetComponent<Bullet>() != null) return;

        PlayerHealth player = hitInfo.GetComponentInParent<PlayerHealth>();
        if (player != null && damageTimer <= 0f)
        {
            player.TakeDamage(damage);
            damageTimer = damageCooldown;
        }
    }
}
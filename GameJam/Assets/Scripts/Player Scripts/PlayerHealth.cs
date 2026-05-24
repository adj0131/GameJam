using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    void Start()
    {
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<HealthBar>();
        }

        currentHealth = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        GetComponent<HitFlash>()?.Flash();
    }

    void Die()
    {
        // put death animation and stuff in later
        Debug.Log("Player Died");
    }
}

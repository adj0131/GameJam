using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private bool isDead = false;

    void Start()
    {
        if (healthBar == null)
            healthBar = GetComponentInChildren<HealthBar>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (!isDead && currentHealth <= 0)
            StartCoroutine(Die());
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        PlayerMovement player = GetComponent<PlayerMovement>();

        // Ignore hits while parrying
        if (player != null && player.isInvincible) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        GetComponent<HitFlash>()?.Flash();
    }

    IEnumerator Die()
    {
        isDead = true;

        // Disable player input & movement
        GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // Wait 2 seconds before game over
        yield return new WaitForSeconds(2f);

        SceneControl sc = FindAnyObjectByType<SceneControl>();
        if (sc != null)
            sc.LoadGameOver();
    }
}
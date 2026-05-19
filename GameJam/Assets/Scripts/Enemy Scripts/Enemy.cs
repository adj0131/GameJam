using UnityEngine;

public class Enemy : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public SpriteRenderer enemySprite;

    public float xPos;
    public float yPos;

    public int health = 100;

    private void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}

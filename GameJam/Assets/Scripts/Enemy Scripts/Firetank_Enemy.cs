using UnityEngine;
using System.Collections;

public class FireTankEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public float moveSpeed = 2f;
    public float attackRange = 5f;
    public float attackCooldown = 2f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public float timeBetweenBullets = 0.15f;

    [Header("Death")]
    public GameObject deathEffectPrefab;    // drag your death animation prefab here
    public float deathDelay = 1f;           // how long before destroying after death anim

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Transform player;
    private bool isDead = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Debug.Log("Player found: " + (player != null)); // temp: confirm player is found
    }

    private void Update()
    {
        if (isDead || player == null) return;

        attackTimer += Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        spriteRenderer.flipX = player.position.x < transform.position.x;

        if (distanceToPlayer <= attackRange)
        {
            myAnimator.SetBool("isRunning", false);

            if (attackTimer >= attackCooldown && !isAttacking)
            {
                attackTimer = 0f;
                StartCoroutine(ShootBurst());
            }
        }
        else
        {
            if (!isAttacking)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );
                myAnimator.SetBool("isRunning", true);
            }
        }
    }

    IEnumerator ShootBurst()
    {
        isAttacking = true;
        myAnimator.SetBool("isRunning", false);
        myAnimator.SetTrigger("Shoot");

        yield return new WaitForSeconds(0.2f);

        FireBullet(firePoint1);
        yield return new WaitForSeconds(timeBetweenBullets);

        FireBullet(firePoint2);
        yield return new WaitForSeconds(timeBetweenBullets);

        FireBullet(firePoint3);
        yield return new WaitForSeconds(timeBetweenBullets);

        yield return new WaitUntil(() =>
            !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireTank_Shoot") ||
            myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        );

        isAttacking = false;
    }

    void FireBullet(Transform firePoint)
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 direction = (player.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        bullet.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;
        Debug.Log("FireTank health: " + health); // temp: confirm damage is registering
        if (health <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        isAttacking = false;

        // stop all movement and shooting
        StopAllCoroutines();

        // disable collider so no more bullets hit it
        if (boxCollider != null)
            boxCollider.enabled = false;

        // spawn death effect on top of firetank
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            effect.GetComponent<SpriteRenderer>().sortingOrder = 15; // render above everything
        }

        // play death animation then destroy
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        // wait for death animation length instead of checking state name
        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }
}
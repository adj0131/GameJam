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
    public float sync = 0.1f; // Time after animation starts to fire first bullet
    public GameObject bulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public float timeBetweenBullets = 0.15f;

    [Header("Death")]
    public GameObject deathEffectPrefab;
    public float deathDelay = 1f;
    [SerializeField] AudioClip firetankDeathClip;
    FiretankSoundManager firetankSoundManager;

    public bool isMoving;
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
        firetankSoundManager = GetComponent<FiretankSoundManager>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Target")?.transform;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        attackTimer += Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Flip sprite to face the player
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-7f, 7f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(7f, 7f, 1f);
        }

        if (distanceToPlayer <= attackRange)
        {
            myAnimator.SetBool("isRunning", false);
            isMoving = false;

            if (!isAttacking && attackTimer >= attackCooldown)
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
                isMoving = true;
            }
        }
    }

    private IEnumerator ShootBurst()
    {
        firetankSoundManager.playShootAudio();
        isAttacking = true;
        myAnimator.SetBool("isRunning", false);
        myAnimator.SetTrigger("Shoot");

        // Brief delay so the shoot animation has started before bullets fire
        yield return new WaitForSeconds(sync);

        FireBullet(firePoint1);
        yield return new WaitForSeconds(timeBetweenBullets);

        FireBullet(firePoint2);
        yield return new WaitForSeconds(timeBetweenBullets);

        FireBullet(firePoint3);

        // Fixed cooldown wait instead of relying on animation state name
        yield return new WaitForSeconds(attackCooldown * 0.5f);

        isAttacking = false;
    }

    private void FireBullet(Transform firePoint)
    {
        if (bulletPrefab == null || firePoint == null || player == null) return;

        // Direction from this specific barrel toward the player
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Spawn bullet at the barrel, no rotation needed — Init() handles direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Sort above other sprites
        SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 10;

        // Pass direction to bullet so it travels correctly regardless of sprite orientation
        FireTankBullet bulletScript = bullet.GetComponent<FireTankBullet>();
        if (bulletScript != null)
        {
            bulletScript.Init(direction);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;
        GetComponent<HitFlash>()?.Flash();
        if (health <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        isAttacking = false;
        FindAnyObjectByType<PlayerGunUpgrades>().hasTripleShot = true;
        StopAllCoroutines();

        if (boxCollider != null)
            boxCollider.enabled = false;

        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            SpriteRenderer sr = effect.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = 15;
        }

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        myAnimator.SetTrigger("Death");
        firetankSoundManager.playDeathAudio();
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
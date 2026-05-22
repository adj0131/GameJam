using UnityEngine;
using System.Collections;

public class FridgeEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int health = 200;
    public float moveSpeed = 2f;
    public float preferredRange = 8f;
    public float tooCloseRange = 5f;
    public float attackRange = 10f;
    public float attackCooldown = 3f;

    [Header("Phase 2 - Chainsaw")]
    public float phase2HealthThreshold = 100f;
    public float chainsawRange = 2f;
    public int chainsawDamage = 20;
    public float chainsawCooldown = 1f;
    public GameObject chainsawArm;
    private bool isPhase2 = false;
    private float chainsawTimer = 0f;

    [Header("Shooting")]
    public GameObject iceProjectilePrefab;
    public Transform firePoint;

    [Header("Death")]
    public GameObject deathEffectPrefab;
    public float deathDelay = 1f;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Transform target;
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
        target = GameObject.FindGameObjectWithTag("Target")?.transform;
    }

    private void Update()
    {
        if (isDead || target == null) return;

        attackTimer += Time.deltaTime;
        chainsawTimer += Time.deltaTime;

        // Distance on X axis only
        float distanceToTarget = Mathf.Abs(transform.position.x - target.position.x);

        // Flip to face target on X axis only
        if (target.position.x < transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);

        // Phase 2 check
        if (!isPhase2 && health <= phase2HealthThreshold)
        {
            isPhase2 = true;
            if (chainsawArm != null)
                chainsawArm.SetActive(true);
        }

        // Phase 2 chainsaw melee attack
        if (isPhase2 && distanceToTarget <= chainsawRange && chainsawTimer >= chainsawCooldown)
        {
            chainsawTimer = 0f;
            ChainsawAttack();
        }

        if (!isAttacking)
        {
            HandleMovement(distanceToTarget);
        }

        // Ranged attack
        if (distanceToTarget <= attackRange && attackTimer >= attackCooldown && !isAttacking)
        {
            attackTimer = 0f;
            StartCoroutine(ShootProjectile());
        }
    }

    private void HandleMovement(float distanceToTarget)
    {
        // Lock Y axis — only move on X
        Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
        Vector2 awayPosition = new Vector2(
            transform.position.x + (transform.position.x - target.position.x),
            transform.position.y
        );

        if (distanceToTarget < tooCloseRange)
        {
            // Back away on X axis only
            transform.position = Vector2.MoveTowards(
                transform.position,
                awayPosition,
                moveSpeed * Time.deltaTime
            );
            myAnimator.SetBool("isWalking", true);
        }
        else if (distanceToTarget > preferredRange)
        {
            // Move toward player on X axis only
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            myAnimator.SetBool("isWalking", true);
        }
        else
        {
            myAnimator.SetBool("isWalking", false);
        }
    }

    private IEnumerator ShootProjectile()
    {
        isAttacking = true;
        myAnimator.SetBool("isWalking", false);
        myAnimator.SetTrigger("Shoot");

        // Wait for freezer door to open in animation
        yield return new WaitForSeconds(0.4f);

        FireIceProjectile();

        // Wait for animation to finish
        yield return new WaitForSeconds(0.6f);

        isAttacking = false;
    }

    private void FireIceProjectile()
    {
        if (iceProjectilePrefab == null || firePoint == null || target == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;

        GameObject projectile = Instantiate(iceProjectilePrefab, firePoint.position, Quaternion.identity);

        SpriteRenderer sr = projectile.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 10;

        IceProjectile iceScript = projectile.GetComponent<IceProjectile>();
        if (iceScript != null)
            iceScript.Init(direction);
    }

    private void ChainsawAttack()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, chainsawRange, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(chainsawDamage);
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
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chainsawRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
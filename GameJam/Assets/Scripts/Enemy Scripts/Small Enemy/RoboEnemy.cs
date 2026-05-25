using UnityEngine;
using System.Collections;

public class RoboEnemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHits = 3;
    private int currentHits;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float attackMoveMultiplier = 0.4f;
    [SerializeField] float tooCloseDistance = 2f;
    [SerializeField] float tooFarDistance = 6f;

    [Header("Shooting")]
    [SerializeField] GameObject lightningProjectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float shootCooldown = 2f;

    [Header("Death")]
    [SerializeField] GameObject deathEffectPrefab;
    [SerializeField] float deathDelay = 1f;
    [SerializeField] AudioClip roboDeathClip;
    AudioSource roboAudioSource;

    [Header("Animator")]
    [SerializeField] Animator animator;

    private Transform player;
    private Rigidbody2D rb;
    private Collider2D col;
    private float shootTimer;
    private bool isAttacking = false;
    private bool isDead = false;

    private enum State { Approach, Retreat, Idle }
    private State currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        roboAudioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Target").transform;
        currentHits = maxHits;
        shootTimer = shootCooldown;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < tooCloseDistance)
            currentState = State.Retreat;
        else if (distanceToPlayer > tooFarDistance)
            currentState = State.Approach;
        else
            currentState = State.Idle;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            isAttacking = true;
            animator.SetTrigger("Shoot");
            shootTimer = shootCooldown;
        }

        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-6, 6, 1);
        else
            transform.localScale = new Vector3(6, 6, 1);
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float speed = isAttacking ? moveSpeed * attackMoveMultiplier : moveSpeed;

        switch (currentState)
        {
            case State.Approach:
                rb.linearVelocity = direction * speed;
                animator.SetBool("isWalking", true);
                break;

            case State.Retreat:
                rb.linearVelocity = -direction * speed;
                animator.SetBool("isWalking", true);
                break;

            case State.Idle:
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("isWalking", false);
                break;
        }
    }

    // Called by Animation Event on frame 1 of Robo_attack
    public void SpawnProjectile()
    {
        if (isDead) return;
        if (lightningProjectilePrefab == null || firePoint == null) return;

        roboAudioSource.Play();
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        GameObject proj = Instantiate(lightningProjectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<LightningProjectile>().SetDirection(direction);

        isAttacking = false;
    }

    public void TakeHit()
    {
        if (isDead) return;
        currentHits--;
        GetComponent<HitFlash>()?.Flash();
        if (currentHits <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        StopAllCoroutines();

        if (col != null)
            col.enabled = false;

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
        animator.SetTrigger("Death");
        if (roboAudioSource != null && roboDeathClip != null)
        {
            roboAudioSource.clip = roboDeathClip;
            roboAudioSource.Play();
        }
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
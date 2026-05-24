using UnityEngine;
using System.Collections;

public class ChainsawEnemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHits = 3;
    private int currentHits;

    [Header("Flipping")]
    [SerializeField] bool facingLeft = false;

    [Header("Death")]
    [SerializeField] GameObject deathEffectPrefab;
    [SerializeField] float deathDelay = 1f;
    [SerializeField] AudioClip deathClip;
    private AudioSource audioSource;

    [Header("Animator")]
    [SerializeField] Animator animator;

    private Collider2D col;
    private bool isDead = false;

    void Start()
    {
        currentHits = maxHits;
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();

        // Flip based on inspector toggle
        if (facingLeft)
            transform.localScale = new Vector3(-3.5f, 3.5f, 1);
        else
            transform.localScale = new Vector3(3.5f, 3.5f, 1);

        // Swing animation plays constantly
        animator.SetBool("isSwinging", true);
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

        animator.SetBool("isSwinging", false);

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
        if (audioSource != null && deathClip != null)
        {
            audioSource.clip = deathClip;
            audioSource.Play();
        }
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
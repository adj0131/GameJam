using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour
{
    public float knockbackForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            if (rb == null || pm == null) return;

            pm.isKnockedBack = true;

            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

            StartCoroutine(ResetKnockback(pm));
        }
    }

    private IEnumerator ResetKnockback(PlayerMovement pm)
    {
        yield return new WaitForSeconds(0.2f);
        pm.isKnockedBack = false;
    }
}
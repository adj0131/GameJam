using UnityEngine;
using System.Collections;

public class IcedEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color iceColor = new Color(0.5f, 0.8f, 1f, 1f); // light blue tint

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void ApplyIce(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(IceTint(duration));
    }

    private IEnumerator IceTint(float duration)
    {
        spriteRenderer.color = iceColor;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }
}
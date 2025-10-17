#pragma warning disable 0618
using System.Collections;
using UnityEngine;

public class MiniMeteorCustomizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer eyeRenderer;
    [SerializeField] private Sprite[] possibleEyeSprites;
    [SerializeField] private SpriteRenderer fireRenderer;

    private Animator animator;
    private CircleCollider2D col;
    private bool canExplode = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();

        
        // náhodné oči
        int randomIndex = Random.Range(0, possibleEyeSprites.Length);
        if (possibleEyeSprites.Length > 0)
            eyeRenderer.sprite = possibleEyeSprites[randomIndex];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Kolízia s: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }
            Explode();
        }
    }

    private void Explode()
    {
        canExplode = false;

        if (col != null)
            col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (animator != null)
            animator.SetTrigger("Explo");

        if (fireRenderer != null)
            fireRenderer.enabled = false;

        if (eyeRenderer != null)
            eyeRenderer.enabled = false;

        Destroy(gameObject, 0.5f); // znič po animácii
    }
}

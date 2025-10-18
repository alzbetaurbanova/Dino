#pragma warning disable 0618
using UnityEngine;

public class MiniMeteorCustomizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer eyeRenderer;
    [SerializeField] private Sprite[] possibleEyeSprites;
    [SerializeField] private SpriteRenderer fireRenderer;

    private Animator animator;
    private CircleCollider2D col;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();

        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            Collider2D bossCollider = boss.GetComponent<Collider2D>();
            if (bossCollider != null && col != null)
            {
                // Prikážeme fyzikálnemu enginu, aby trvalo ignoroval kolíziu medzi meteoritom a bossom
                Physics2D.IgnoreCollision(col, bossCollider, true);
            }
        }

        if (possibleEyeSprites.Length > 0 && eyeRenderer != null)
        {
            int randomIndex = Random.Range(0, possibleEyeSprites.Length);
            eyeRenderer.sprite = possibleEyeSprites[randomIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Debug.Log("Kolízia s: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);

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
        //Debug.Log("Explózia na: " + gameObject.name);

        if (col != null)
            col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (fireRenderer != null)
            fireRenderer.enabled = false; // Vypne OHEŇ
        if (eyeRenderer != null)
            eyeRenderer.enabled = false; // Vypne OČI

        if (animator != null)
            animator.SetTrigger("Explo");
        else
            Debug.LogWarning("no animation");

        Destroy(gameObject, 0.71f);
    }
}

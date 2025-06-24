#pragma warning disable 0618
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    [SerializeField] private GameObject Explosion;

    void Update() => transform.right = rb.velocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            Instantiate(Explosion, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            UI.instance.AddScore();

            TargetSpawner spawner = FindObjectOfType<TargetSpawner>();
            if (spawner != null)
                spawner.TargetDestroyed();
        }
        else if (collision.CompareTag("Boss"))
        {
            MeteorBossController boss = collision.GetComponent<MeteorBossController>();
            if (boss != null)
            {
                boss.TakeDamage(1); //
            }

            Destroy(gameObject); // znič len strelu, NIE výbuch
        }
    }

}

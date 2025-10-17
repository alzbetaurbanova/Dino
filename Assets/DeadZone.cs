#pragma warning disable 0618
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private bool isDeadZoneActive = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDeadZoneActive)
            return;

        if (collision.CompareTag("Target")) // teda meteorit
        {
            // Najdi hráča a uber mu HP
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }

            Destroy(collision.gameObject); // meteorit zmizne
        }
        else if (collision.CompareTag("Player"))
        {
            // Ak padne sám hráč (napr. mimo mapy), rovno uber HP
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }

}


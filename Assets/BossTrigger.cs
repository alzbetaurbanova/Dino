#pragma warning disable 0618 //vypne upozornenia
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyTriggered) return;
        if (other.CompareTag("Player"))
        {
            alreadyTriggered = true;
            FindObjectOfType<MeteorBossController>().StartBossFightExternally();
            Destroy(gameObject); // zničí trigger po použití
        }
    }
}

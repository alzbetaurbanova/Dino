#pragma warning disable 0618
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UI.instance.UpdateHPHearts(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UI.instance.UpdateHPHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        UI.instance.OpenEndScreen();
    }
}

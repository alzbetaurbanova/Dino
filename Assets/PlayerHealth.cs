#pragma warning disable 0618
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (UI.instance != null)
            UI.instance.UpdateHPHearts(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (UI.instance != null)
            UI.instance.UpdateHPHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (UI.instance != null)
            UI.instance.OpenEndScreen();
    }
}

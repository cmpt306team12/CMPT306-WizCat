using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the current health to be the max health
        currentHealth = maxHealth;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Game Object has died.");
            // Play death animation
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // Check whether the current health exceeds the max health
        if (currentHealth > maxHealth)
        {
            // Ensure current health does not exceed max health
            currentHealth = maxHealth;
        }
    }
}

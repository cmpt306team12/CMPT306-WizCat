using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the current health to be the max health
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            Debug.Log("Game Object has died.");
            // Play death animation
        }
    }

    public void Heal(float amount)
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

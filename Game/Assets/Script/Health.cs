using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set both the player and enemy's current health to be the max health
        player.currentHealth = player.maxHealth;
        enemy.currentHealth = enemy.maxHealth;
        
    }

    public void DamageToPlayer(float damageAmount)
    {
        player.currentHealth -= damageAmount;

        if (player.currentHealth <= 0.0f)
        {
            Debug.Log("Game Object has died.");
            // Play death animation
        }
    }

    public void HealPlayer(float amount)
    {
        player.currentHealth += amount;

        // Check whether the current health exceeds the max health
        if (player.currentHealth > player.maxHealth)
        {
            // Ensure current health does not exceed max health
            player.currentHealth = player.maxHealth;
        }
    }
    
    public void DamageToEnemy(float damageAmount)
    {
        enemy.currentHealth -= damageAmount;

        if (enemy.currentHealth <= 0.0f)
        {
            Debug.Log("Game Object has died.");
            // Play death animation
        }
    }
    
    public void HealEnemy(float amount)
    {
        enemy.currentHealth += amount;

        // Check whether the current health exceeds the max health
        if (enemy.currentHealth > enemy.maxHealth)
        {
            // Ensure current health does not exceed max health
            enemy.currentHealth = enemy.maxHealth;
        }
    }
}

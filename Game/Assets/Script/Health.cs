using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth; 
    public int EnemyScore = 50;

    // Start is called before the first frame update
    void Start()
    {
        // maxHealth = StaticData.maxHealth;
        currentHealth = StaticData.currentHealth;
        //hurtSFX = GetComponent<AudioSource>();
    }

    public void ApplyDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            Debug.Log(gameObject.tag + " has died.");
            
            if (gameObject.CompareTag("Enemy"))
            {
                GameManager.instance.IncreaseScore(EnemyScore);
            }

            // Play death animation


            Destroy(gameObject);
        }
    }

    public void ApplyHealing(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            Debug.Log(gameObject.tag + " max health.");
        }
    }
}

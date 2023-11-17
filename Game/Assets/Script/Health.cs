using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth; 
    public int EnemyScore = 50;
    public bool dropsLoot = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Player")) {
            // maxHealth = StaticData.maxHealth;
            currentHealth = StaticData.currentHealth;
        }

        else { currentHealth = maxHealth; }
        
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
                // Handle killing an enemy
                GameManager.instance.IncreaseScore(EnemyScore);
                GameManager.instance.EnemyDefeated();
                Destroy(gameObject);
            }else if (gameObject.CompareTag("Obstacle"))
            {
                // if Gameobject is an obstacle, drop loot if it does, then destroy
                if (dropsLoot)
                {
                    gameObject.GetComponent<DropOnDestroy>().Drop();
                }
                Destroy(gameObject);
            } else if (gameObject.CompareTag("Player"))
            {
                // Handle killing player

                // Play death animation
                animator.SetBool("IsDead", true);

                transform.gameObject.tag = "Untagged";
                GetComponent<Collider2D>().enabled = false;
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<OrbitProjectiles>().enabled = false;
                GetComponent<Bite>().enabled = false;

                this.enabled = false;
            } else
            {
                // Unhandled object with health of zero
                Debug.Log("Gameobject has health zero: " + gameObject.name);
            }
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

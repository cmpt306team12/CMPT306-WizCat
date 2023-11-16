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
        Debug.Log("Reduce health by: " + damageAmount);

        if (currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            Debug.Log(gameObject.tag + " has died.");
            
            if (gameObject.CompareTag("Enemy"))
            {
                GameManager.instance.IncreaseScore(EnemyScore);
            }

            // Play death animation
            if (dropsLoot)
            {
                gameObject.GetComponent<DropOnDestroy>().Drop();
            }
            animator.SetBool("IsDead", true);

            transform.gameObject.tag = "Untagged";
            GetComponent<Collider2D>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<OrbitProjectiles>().enabled = false;
            GetComponent<Bite>().enabled = false;




            this.enabled = false;
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

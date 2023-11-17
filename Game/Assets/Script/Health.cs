using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth; 
    public int EnemyScore = 50;

    public Animator animator;
    public AudioSource audioSource;
    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //hurtSFX = GetComponent<AudioSource>();
    }

    public void ApplyDamage(float damageAmount)
    {
        int chooseSound = Random.Range(1, 3); 
        if (chooseSound == 1)
        {
            audioSource.PlayOneShot(hurtSFX1);
        } 
        else
        {
            audioSource.PlayOneShot(hurtSFX2);
        }
        
        animator.SetTrigger("IsHurt");
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
            animator.SetBool("IsDead", true);

            transform.gameObject.tag = "Untagged";
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<Collider2D>().enabled = false; 

            // Disable Player Scripts
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<OrbitProjectiles>().enabled = false;
            GetComponent<Bite>().enabled = false;

            // Disable Enemy Scripts
            GetComponent<Enemy>().enabled = false;


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

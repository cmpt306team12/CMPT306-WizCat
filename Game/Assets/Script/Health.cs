using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int EnemyScore = 50;
    public bool dropsLoot = false;

    public Animator animator;
    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            // maxHealth = StaticData.maxHealth;
            currentHealth = StaticData.currentHealth;
        }

        else { currentHealth = maxHealth; }

        //hurtSFX = GetComponent<AudioSource>();
    }

    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }

    public void ApplyDamage(float damageAmount)
    {
        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy"))
        {
            int chooseSound = Random.Range(1, 3);
            if (chooseSound == 1)
            {
                gameObject.GetComponent<RandomSound>().PLayClipAt(hurtSFX1, transform.position);
            }
            else
            {
                gameObject.GetComponent<RandomSound>().PLayClipAt(hurtSFX2, transform.position);
            } 
            animator.SetTrigger("IsHurt");
        }


        currentHealth -= damageAmount;

        if (currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            Debug.Log(gameObject.tag + " has died.");

            if (gameObject.CompareTag("Enemy"))
            {
                if (dropsLoot)
                {
                    gameObject.GetComponent<DropOnDestroy>().Drop();
                }
                // Handle killing an enemy
                GameManager.instance.IncreaseScore(EnemyScore);
                GameManager.instance.EnemyDefeated();
                animator.SetBool("IsDead", true);
                gameObject.GetComponent<Enemy>().enabled = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
                gameObject.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (gameObject.CompareTag("Obstacle"))
            {
                // if Gameobject is an obstacle, drop loot if it does, then destroy
                if (dropsLoot)
                {
                    gameObject.GetComponent<DropOnDestroy>().Drop();
                }
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("Player"))
            {
                // Handle killing player

                // Play death animation
                animator.SetBool("IsDead", true);

                transform.gameObject.tag = "Untagged";
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<OrbitProjectiles>().enabled = false;
                GetComponent<Bite>().enabled = false;
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
                gameObject.transform.GetChild(3).gameObject.SetActive(false);

                this.enabled = false;

                StartCoroutine(PlayerDeath());
                StaticData.Reset();

            }
            else
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

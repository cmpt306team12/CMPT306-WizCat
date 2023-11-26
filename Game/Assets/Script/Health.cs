using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Health : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int EnemyScore = 50;
    public bool dropsLoot = false;

    public Animator animator;
    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;

    public GameObject EnemyFloatingTextPrefab;
    public GameObject AppraisalFloatingTextPrefab;
    public string[] deathQuotes;
    public string[] appraisalQuotes;

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
        }

        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy"))
        {
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



                // 30% chance to drop an enemy message
                deathQuotes = new string[0];
                List<string> tempList = new List<string>(deathQuotes);
                tempList.AddRange(new string[]{
                "My monitor was unplugged",
                "You're just a cat...",
                "Friendly reminder: life is meant to be lived. Unless it's your life, in which case it was meant to given to me.",
                "They say the definition of insanity is repeating the same option over and over again and expecting different results. But hear me out - there's no way you're going to kill me again.",
                "Please, we don’t believe you should be swagging that hard ok, listen, we worry about your hands, feet, head, ankles, arms, and face.",
                "Are you hungry?",
                "I'm cooking something up for you ...",
                "Yum, yum, yum!",
                "Fried okra, raw beans, washed mushrooms, raw peppers, dank cheese, astringent spinach, moldy celery ...",
                "Acidic bananas, appealing grapes, appetizing yams, aromatic chard, balsamic watermelon, beautiful wheatgrass, blazed carrots ...", 
                "Blended eggs, blunt romaine, boiled bacon, briny basil, burnt bread, caustic  garlic, cold pasta, chunky salt, creamy water, crispy oranges ...", 
                "Crunchy nectarines, cured peaches, delectable apricots, delightful basil, doughy apples, drenched raisins, dry plums, elastic turkey ...",
                });
                deathQuotes = tempList.ToArray();

                float randomChance = Random.value;
                Vector3 offset = new Vector3(0.0f, 2.0f, 0.0f);
                GameObject text = Instantiate(EnemyFloatingTextPrefab, gameObject.transform.position + offset, Quaternion.identity);
                TextMeshProUGUI atextMesh = text.GetComponentInChildren<TextMeshProUGUI>();
                string aText = (randomChance < 0.3f) ? deathQuotes[Random.Range(0, deathQuotes.Length)] : string.Empty;
                // string aText = deathQuotes[Random.Range(0, deathQuotes.Length)];
                atextMesh.text = aText;
                if (!string.IsNullOrEmpty(aText))
                {
                    Destroy(text, 10.0f);
                }

                // 50% chance to drop an appraisal message
                appraisalQuotes = new string[0];
                List<string> tempList2 = new List<string>(appraisalQuotes);
                tempList2.AddRange(new string[]{
                    "Great Job!",
                    "Keep it Up!",
                    "Wow!",
                    "Catastrophic!",
                    "Purfect!",
                    "Claw-some!",
                    "Toxoplasmotic!",
                    "In-Festive!",
                    "Ohm Wrecker!",
                    "Magical cadence!",
                    "Arcane Spellbind!",
                    "Gimbal and Gyre!"
                });
                appraisalQuotes = tempList2.ToArray();

                float randomChance2 = Random.value;
                Vector3 offset2 = new Vector3(0.0f, -1.0f, 0.0f);
                GameObject text2 = Instantiate(AppraisalFloatingTextPrefab, gameObject.transform.position + offset2, Quaternion.identity);
                TextMeshProUGUI textMesh2 = text2.GetComponentInChildren<TextMeshProUGUI>();
                string Text2 = (randomChance2 < 0.5f) ? appraisalQuotes[Random.Range(0, appraisalQuotes.Length)] : string.Empty;
                // string Text2 = appraisalQuotes[Random.Range(0, appraisalQuotes.Length)];
                textMesh2.text = Text2;
                if (!string.IsNullOrEmpty(Text2))
                {
                    StartCoroutine(ShimmerText(textMesh2));
                    Destroy(text2, 3.0f);
                }


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
                
                GameManager.instance.SaveHighScores();

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


    public IEnumerator ShimmerText(TextMeshProUGUI textMesh)
    {
        float time = 0.0f;
        float duration = 2.0f; 
        float riseSpeed = 0.5f;
        Vector3 originalPosition = textMesh.transform.position;

        while (time < duration)
        {
            float alpha = Mathf.PingPong(time * 2.0f, 1.0f);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
            textMesh.transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0.0f);
        textMesh.transform.position = originalPosition;
    }

}
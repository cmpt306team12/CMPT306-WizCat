using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Pathfinding;
using UnityEngine.UIElements;
//using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int EnemyScore = 50;
    public bool dropsLoot = false;
    public bool isImmune = false;
    public float immunityDuration = 0.5f;

    public Animator animator;
    public AudioClip hurtSFX1;
    public AudioClip hurtSFX2;

    public GameObject EnemyFloatingTextPrefab;
    public GameObject AppraisalFloatingTextPrefab;
    public string[] deathQuotes;
    public string[] appraisalQuotes;

    //Should only be used by the player
    public CanvasGroup gameOverScreen;
    public bool fadeIn = false;

    // Heal particle effect gameobject
    public GameObject healPS;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            maxHealth = StaticData.maxHealth;
            currentHealth = StaticData.currentHealth;
        }

        else { currentHealth = maxHealth; }
        //hurtSFX = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (gameOverScreen.alpha < 1)
            {
                gameOverScreen.alpha += Time.deltaTime;
                if (gameOverScreen.alpha >= 1)
                {
                    fadeIn = false;
                    this.enabled = false;  //Disables "Health" script
                }
            }
        }
    }

    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSeconds(4.0f);
        gameOverScreen.gameObject.SetActive(true);
        fadeIn = true;
    }

    private IEnumerator RedHurt()
    {
        //getting the wands might be a little scuffed
        gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
       
    }

    private IEnumerator GreenHeal()
    {
        //getting the wands might be a little scuffed
        gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        healPS.SetActive(true);
        healPS.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.3f);
        healPS.GetComponent<ParticleSystem>().Stop();

        gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(2.0f);
        healPS.SetActive(false);

    }

    public void ApplyDamage(float damageAmount)
    {
        if (!isImmune)
        {
            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy"))
            {
                int chooseSound = UnityEngine.Random.Range(1, 3);
                if (chooseSound == 1)
                {
                    gameObject.GetComponent<RandomSound>().PLayClipAt(hurtSFX1, transform.position);
                }
                else
                {
                    gameObject.GetComponent<RandomSound>().PLayClipAt(hurtSFX2, transform.position);
                }

                StartCoroutine(RedHurt());
                animator.SetTrigger("IsHurt");
            }

            if (gameObject.CompareTag("Obstacle") && gameObject.GetComponent<ExplosiveBarrel>() != null)
            {
                // Explosive barrel - set off fuze
                if (!gameObject.GetComponent<ExplosiveBarrel>().IsLit())
                {
                    gameObject.GetComponent<ExplosiveBarrel>().LightFuse();
                }
            }

            // player takes less damage at low health
            if (gameObject.CompareTag("Player"))
            {
                if (currentHealth / maxHealth <= .33)
                {
                    float reducedDamage = damageAmount * 0.66f;
                    int roundedDamage = (int)Mathf.Round(reducedDamage);
                    if (roundedDamage < 1) { roundedDamage = 1; }
                    currentHealth -= roundedDamage;
                }
                else
                {
                    currentHealth -= damageAmount;
                    StartCoroutine(ToggleImmunity(immunityDuration));
                }
            }
            // when anything other than the player takes damage
            else
            {
                currentHealth -= damageAmount;
                if (gameObject.CompareTag("Player")) { StartCoroutine(ToggleImmunity(immunityDuration)); }
            }

            // death
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
                "My monitor was unplugged...",
                "You're just a cat...",
                "Yum, yummm...",
                "Next timee...",
                "Uggghh...",
                "Gaahhhh...",
                "Not again.....",
                "Why......",
                "I hate cats....",
                "Nice shot....",
                "Owww.....",
                "You're stuck here....",
                "You can't leave....",
                "nice tr....",
                "damn lag....",
                "see you soon....",
                "Tell my fr.....",
                "soon...",
                "how did...",
                "AAaahhh.....",
                "My mouse was unplugged...",
                "You're only a cat...",
                "Are you hungry?",
                "I'm cooking something up for you ..."
                });
                    deathQuotes = tempList.ToArray();

                    float randomChance = UnityEngine.Random.value;
                    Vector3 offset = new Vector3(0.0f, 2.0f, 0.0f);
                    GameObject text = Instantiate(EnemyFloatingTextPrefab, gameObject.transform.position + offset, Quaternion.identity);
                    TextMeshProUGUI atextMesh = text.GetComponentInChildren<TextMeshProUGUI>();
                    string aText = (randomChance < 0.3f) ? deathQuotes[Random.Range(0, deathQuotes.Length)] : string.Empty;
                    //string aText = deathQuotes[UnityEngine.Random.Range(0, deathQuotes.Length)];
                    atextMesh.text = aText;
                    if (!string.IsNullOrEmpty(aText))
                    {
                        StartCoroutine(WiggleText(atextMesh, text));
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

                    float randomChance2 = UnityEngine.Random.value;
                    Vector3 offset2 = new Vector3(0.0f, -1.0f, 0.0f);
                    GameObject text2 = Instantiate(AppraisalFloatingTextPrefab, gameObject.transform.position + offset2, Quaternion.identity);
                    TextMeshProUGUI textMesh2 = text2.GetComponentInChildren<TextMeshProUGUI>();
                    string Text2 = (randomChance2 < 0.5f) ? appraisalQuotes[UnityEngine.Random.Range(0, appraisalQuotes.Length)] : string.Empty;
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
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    gameObject.GetComponent<Enemy>().CancelInvoke();
                    Destroy(gameObject.GetComponent<Enemy>().stun);
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    gameObject.transform.GetChild(3).gameObject.SetActive(false);
                    gameObject.transform.GetChild(4).gameObject.SetActive(false);
                    gameObject.transform.GetChild(5).gameObject.SetActive(false);
                }
                else if (gameObject.CompareTag("Obstacle"))
                {
                    if (gameObject.GetComponent<ExplosiveBarrel>() != null)
                    {
                        // Make explosive barrel explode
                        gameObject.GetComponent<ExplosiveBarrel>().Explode();
                    }
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
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    gameObject.GetComponent<Player>().CancelInvoke();
                    gameObject.GetComponentInChildren<Wand>().enabled = false;
                    Destroy(gameObject.GetComponent<Player>().stun);
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    gameObject.transform.GetChild(3).gameObject.SetActive(false);
                    gameObject.transform.GetChild(4).gameObject.SetActive(false);
                    gameObject.transform.GetChild(5).gameObject.SetActive(false);

                    StartCoroutine(PlayerDeath());
                    StaticData.Reset();

                }
                else
                {
                    // Unhandled object with health of zero
                    Debug.Log("Gameobject has health zero: " + gameObject.name);
                }

                // disable minimap icon

                GameObject minimapIcon = FindMinimapInChildren();

                // Check if its null
                if (minimapIcon != null)
                {
                    minimapIcon.SetActive(false);
                }
            }
        }
    }

    // immunity toggle
    private IEnumerator ToggleImmunity(float duration)
    {
        isImmune = true;

        yield return new WaitForSeconds(duration);

        isImmune = false;
    }

    public void ApplyHealing(float healAmount)
    {
        currentHealth += healAmount;
        if (gameObject.CompareTag("Player"))
        {
            StartCoroutine(GreenHeal());
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            Debug.Log(gameObject.tag + " max health.");
        }
    }

    public IEnumerator WiggleText(TextMeshProUGUI textMesh, GameObject text)
    {
        float alpha = 0.0f;
        float fadeInTime = 1.0f;
        while (alpha < 1.0) // fade in
        {
            textMesh.ForceMeshUpdate();
            Mesh mesh = textMesh.mesh;
            Vector3[] vertices = mesh.vertices;
            float time = Time.time;
            for (int i = 0; i < textMesh.textInfo.characterCount; i++)
            {

                TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];
                int index = c.vertexIndex;
                Vector3 offset = Wiggle(time + i);
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
            mesh.vertices = vertices;
            textMesh.fontMaterial.SetColor("_FaceColor", new Color(1, 1, 1, alpha));
            textMesh.fontMaterial.SetColor("_OutlineColor", new Color(0, 0, 0, alpha));
            alpha = alpha + (Time.deltaTime / fadeInTime);
            textMesh.canvasRenderer.SetMesh(mesh);
            yield return null;
        }
        float fadeTime = 4.0f;
        while (alpha > 0.0) // fade out
        {
            textMesh.ForceMeshUpdate();
            Mesh mesh = textMesh.mesh;
            Vector3[] vertices = mesh.vertices;
            float time = Time.time;
            for (int i = 0; i < textMesh.textInfo.characterCount; i++)
            {
                
                TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];
                int index = c.vertexIndex;
                Vector3 offset = Wiggle(time + i);
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }
            mesh.vertices = vertices;
            textMesh.fontMaterial.SetColor("_FaceColor", new Color(1, 1, 1, alpha));
            textMesh.fontMaterial.SetColor("_OutlineColor", new Color(0, 0, 0, alpha));
            alpha = alpha - (Time.deltaTime / fadeTime);
            textMesh.canvasRenderer.SetMesh(mesh);
            yield return null;
        }
        Destroy(text);
    }

    Vector2 Wiggle(float time)
    {
        return new Vector2(0, Mathf.Sin(time));
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

    // Function to find the "Minimap" tag in children
    GameObject FindMinimapInChildren()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.CompareTag("Minimap"))
            {
                return child.gameObject;
            }
        }

        // Return null if "Minimap" tag is not found in children
        return null;
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }
}
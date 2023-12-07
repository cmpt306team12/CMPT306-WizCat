using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrayHeal : MonoBehaviour
{
    private bool hasPrayed = false;
    [SerializeField] public float healAmount = 20;
    private GameManager gameManager;
    private GameObject player;

    // heal sound
    public AudioClip healSound;
    public GameObject godRays;
    public GameObject leafCircle;

    public GameObject AccessFloatingTextPrefab;
    public TextMeshProUGUI secretFoundText;
    // player pref secret
    private string secretFoundKey = "SecretHealFound";

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = gameManager.GetPlayer();
    }

    public void Pray()
    {
        if (!hasPrayed)
        {
            player.GetComponentInChildren<AudioSource>().clip = healSound;
            player.GetComponentInChildren<AudioSource>().Play();
            player.GetComponent<Health>().ApplyHealing(healAmount);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            hasPrayed = true;

            StartCoroutine(SecretFound());

            godRays.GetComponent<ParticleSystem>().Stop();
            leafCircle.GetComponent<ParticleSystem>().Stop();
        }
    }

    public IEnumerator SecretFound()
    {
        // check if found before
        if (PlayerPrefs.GetInt(secretFoundKey, 0) == 0)
        {
            // Display reward of access and stop particle effect
            Vector3 offset = new Vector3(0.0f, -1.0f, 0.0f);
            GameObject text = Instantiate(AccessFloatingTextPrefab, gameObject.transform.position + offset, Quaternion.identity);
            TextMeshProUGUI textMesh = text.GetComponentInChildren<TextMeshProUGUI>();
            string Text = "Secret Found!";
            textMesh.text = Text;
            Destroy(text, 3.0f);

            PlayerPrefs.SetInt(secretFoundKey, 1);
            PlayerPrefs.Save();
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
}

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
    public GameObject stoneHealPS;

    public GameObject AccessFloatingTextPrefab;
    public TextMeshProUGUI secretFoundText;
    // player pref secret
    private string secretFoundKey = "SecretHealFound";

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = gameManager.GetPlayer();

        stoneHealPS.SetActive(true);
        stoneHealPS.GetComponent<ParticleSystem>().Play();
    }

    public void Pray()
    {
        if (!hasPrayed)
        {
            player.GetComponentInChildren<AudioSource>().clip = healSound;
            player.GetComponentInChildren<AudioSource>().Play();
            player.GetComponent<Health>().ApplyHealing(healAmount);
            Destroy(gameObject);
            hasPrayed = true;

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
            }

            stoneHealPS.SetActive(false);
        }
    }
}

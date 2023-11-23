using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayHeal : MonoBehaviour
{
    private bool hasPrayed = false;
    [SerializeField] public float healAmount = 20;
    private GameManager gameManager;
    private GameObject player;

    // heal sound
    public AudioClip healSound;

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
            Destroy(gameObject);
            hasPrayed = true;
        }
    }
}

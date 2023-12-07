using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    [SerializeField] public float healAmount = 25;
    private GameManager gameManager;
    private GameObject player;

    // pickup sound
    public AudioClip healSound;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = gameManager.GetPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponentInChildren<AudioSource>().clip = healSound;
            other.GetComponentInChildren<AudioSource>().Play();
            Destroy(gameObject);
            player.GetComponent<Health>().ApplyHealing(healAmount);
        }
    }
}

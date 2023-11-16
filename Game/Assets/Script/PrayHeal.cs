using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayHeal : MonoBehaviour
{
    private bool hasPrayed = false;
    [SerializeField] public float healAmount = 20;
    private GameManager gameManager;
    private GameObject player;

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
            player.GetComponent<Health>().ApplyHealing(healAmount);
            Destroy(gameObject);
            hasPrayed = true;
        }
    }
}

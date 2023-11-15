using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour { 

    public int value;
    // private CoinCounter counter;
    private GameManager gameManager;

    public AudioSource coinSFX;

    // Start is called before the first frame update
    void Start()
    {
        // counter = GameManager.instance.GetComponent<CoinCounter>();
        gameManager = GameManager.instance;
        coinSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coinSFX.Play();
            Destroy(gameObject);
            // counter.IncreaseCoins(value);
            gameManager.IncreaseCoins(value);

        }
    }
}

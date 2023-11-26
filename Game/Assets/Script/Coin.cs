using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour { 

    public int value;
    // private CoinCounter counter;
    private GameManager gameManager;
    public float attractionRange = 3.0f;
    private Transform playerTransform;
    public float movementSpeed = 5.0f;

    // pickup sound
    public AudioClip coinSound;
    
    // Start is called before the first frame update
    void Start()
    {
        // counter = GameManager.instance.GetComponent<CoinCounter>();
        gameManager = GameManager.instance;
        playerTransform = GameManager.instance.GetPlayer().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTransform.gameObject.GetComponent<Collider2D>().enabled)
        {
            MoveTowardsPlayer();
        }
    }


    private void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= attractionRange)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.Translate(directionToPlayer * Time.deltaTime * movementSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponentInChildren<AudioSource>().clip = coinSound;
            other.GetComponentInChildren<AudioSource>().Play();
            Destroy(gameObject);
            // counter.IncreaseCoins(value);
            gameManager.IncreaseCoins(value);

        }
    }
}

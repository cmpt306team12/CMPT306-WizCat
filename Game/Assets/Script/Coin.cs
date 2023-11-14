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
    
    // Start is called before the first frame update
    void Start()
    {
        // counter = GameManager.instance.GetComponent<CoinCounter>();
        gameManager = GameManager.instance;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
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
            Destroy(gameObject);
            // counter.IncreaseCoins(value);
            gameManager.IncreaseCoins(value);

        }
    }
}

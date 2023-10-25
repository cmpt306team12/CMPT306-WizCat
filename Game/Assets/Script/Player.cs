using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxHealth = 100.0f;

    public float currentHealth = 100.0f;

    public float movementSpeed = 5.0f;

    public float damage;
    public float fireRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getSpeed()
    {
        return movementSpeed;
    }
}

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
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInChildren<Wand>().Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Perk"))
        {
            int id = collision.gameObject.GetComponent<Perk>().GetPerkID();
            collision.gameObject.GetComponent<Perk>().Despawn();
            gameObject.GetComponent<Perks>().AddPerk(id);
        }
    }

    public float getSpeed()
    {
        return movementSpeed;
    }
}

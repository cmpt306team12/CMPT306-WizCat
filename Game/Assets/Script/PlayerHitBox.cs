using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public float damageAmount = 10.0f;
    
    public Health modifyPlayerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("The Player took " + damageAmount + " damage.");
            modifyPlayerHealth.DamageToPlayer(damageAmount);
        }
    }
    
}

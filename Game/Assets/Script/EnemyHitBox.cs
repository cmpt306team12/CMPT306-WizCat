using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public float damageAmount = 10.0f;
    public Health modifyEnemyHealth;

    
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    // // TODO for when the projectiles are finished
    // if (other.CompareTag("Player"))
    //     {
    //         Debug.Log("The Enemy took " + damageAmount + " damage.");
    //         modifyEnemyHealth.DamageToEnemy(damageAmount);
    //     }
    // }
}

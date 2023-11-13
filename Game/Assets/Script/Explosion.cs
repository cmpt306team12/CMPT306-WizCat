using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionDamage = 0.0f;
    private void Start()
    {
        gameObject.GetComponent<RandomSound>().PlayRandomizedSound();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.GetComponent<Health>().ApplyDamage(explosionDamage);
            // Apply Knockback too?
        }

    }

    public void SetDamage(float damage)
    {
        this.explosionDamage = damage;
    }


}

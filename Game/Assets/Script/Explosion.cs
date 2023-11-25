using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionDamage = 0.0f;
    public float knockback = 0.0f;
    public AudioClip explosionClip;
    private void Start()
    {
        gameObject.GetComponent<RandomSound>().PLayClipAt(explosionClip, transform.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            
            if (collision.gameObject.CompareTag("Player"))
            {
                // Stun player so knockback can be applied
                Debug.Log("Stunned Player");
                collision.gameObject.GetComponent<Player>().Stun();
            } else if (collision.gameObject.CompareTag("Enemy"))
            {
                // Stun enemy
            }

            // Ally the knockback to the RigidBody2D
            float scaleFactor = 1.0f;
            if (collision.GetComponent<Rigidbody2D>() != null)
            {
                float radius = gameObject.GetComponent<CircleCollider2D>().radius * gameObject.transform.localScale.x;
                Vector2 center = gameObject.GetComponent<CircleCollider2D>().bounds.center;
                float scaledKnockback = knockback;
                float dist = Vector2.Distance(center, collision.gameObject.GetComponent<Collider2D>().bounds.ClosestPoint(center));
                if (dist > (radius / 2))
                {
                    scaleFactor = Mathf.Clamp((((1.0f - (dist / radius)) / 0.5f) * 0.75f) + 0.25f, 0.25f, 1.0f);
                    scaledKnockback = scaleFactor * knockback;
                    Debug.Log("F: " + scaleFactor + " D: " + dist + "P: " + (dist/radius));
                }
                collision.gameObject.GetComponent<Health>().ApplyDamage(Mathf.Floor(explosionDamage * scaleFactor));
                Vector2 knockbackDirection = (collision.GetComponent<Collider2D>().bounds.center - gameObject.transform.position).normalized;
                collision.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * scaledKnockback, ForceMode2D.Impulse);
            }
        }

    }

    public void SetDamage(float damage)
    {
        this.explosionDamage = damage;
    }

    public void SetKnockback(float kb)
    {
        this.knockback = kb;
    }


}

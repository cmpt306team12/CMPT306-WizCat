using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Rigidbody2D rb; //projectiles rigidbody
    public GameObject deathEffect; // Effect on projectile death
    public GameObject trail; //Trail child object

    public float baseSpeed = 2.0f;
    public float baseLifetime = 3.0f;
    public int bounces = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnCoroutine(baseLifetime));
    }

    IEnumerator DespawnCoroutine(float waitTime)
    {
        // wait for specified time
        yield return new WaitForSeconds(waitTime);
        Despawn();
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate sprite to face direction of travel
        Vector2 rotation = rb.velocity.normalized;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    public void Despawn()
    {
        Instantiate(deathEffect,transform.position, transform.rotation);
        trail.transform.parent = null;
        trail.GetComponent<TrailRenderer>().autodestruct = true;
        Destroy(gameObject);
        Destroy(trail);
    }

    /** Fire()
     * Applies the impulse to fire the projectile, depends on the projectiles stats.
     * */
    public void Fire()
    {
        rb.velocity = transform.right * baseSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            collision.gameObject.GetComponent<Projectile>().Despawn();
            Despawn();
        } else if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            if (bounces == 0)
            {
                Despawn();
            }
            bounces--;
        }
    }
}

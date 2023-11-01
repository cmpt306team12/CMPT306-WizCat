using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{

    public ProjectileProperties ProjectileProperties;

    public Rigidbody2D rb; //projectiles rigidbody
    public GameObject deathEffect; // Effect on projectile death
    public GameObject trail; //Trail child object

    // Local variables needed for applying perks over projectile lifetime
    private bool despawning = false;
    private int bouncesLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator DespawnCoroutine(float waitTime)
    {
        // wait for specified time
        yield return new WaitForSeconds(waitTime);
        Despawn("Coroutine");
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

    public void Despawn(string from)
    {
        despawning = true;
        Debug.Log(from);
        GameObject effect = Instantiate(deathEffect,transform.position, transform.rotation);
        float scale = ProjectileProperties.getScale();
        effect.transform.localScale = new Vector3(scale, scale, scale);
        trail.transform.parent = null;
        trail.GetComponent<TrailRenderer>().autodestruct = true;
        Destroy(gameObject);
    }

    /** Fire()
     * Applies the impulse to fire the projectile, depends on the projectiles stats.
     * */
    public void Fire(ProjectileProperties prop)
    {
        this.ProjectileProperties = prop;
        rb.velocity = transform.right * prop.getSpeed();
        StartCoroutine(DespawnCoroutine(prop.getLifetime()));
        ApplyScale(prop.getScale());
        bouncesLeft = prop.getBounces();
    }

    private void ApplyScale(float scale)
    {
        Debug.Log("Scale: " + scale);
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Projectile") && !despawning)
        {
            Despawn("Trigger Proj");
        } else if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            Debug.Log("Collision");
            this.bouncesLeft = this.bouncesLeft - 1;
            if (this.bouncesLeft < 0)
            {
                Despawn("Trigger Wall/Obs");
            }
            
            
        }
    }
}

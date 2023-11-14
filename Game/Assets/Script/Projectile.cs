using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{

    public ProjectileProperties projProp;

    public Rigidbody2D rb; //projectiles rigidbody
    public GameObject deathEffect; // Effect on projectile death
    public GameObject explosion; // Explosion created if projectile is explosive
    public GameObject trail; //Trail child object
    public GameObject baseProjectile; // Base projectile for burst shots
    public AudioClip destroySound;
    public AudioClip bounceSound;

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

    private IEnumerator SplitCoroutine()
    {
        float splitInterval = 0.5f;
        float remainingLifetime = projProp.getLifetime(); 
        while (remainingLifetime > 0)
        {
            yield return new WaitForSeconds(Mathf.Min(splitInterval, remainingLifetime));
            Split();
            remainingLifetime -= splitInterval;
        }
    }


    private void Split()
    {
        int splits = projProp.getSplits();
        if (splits > 0)
        {
            float angleIncrement = 360f / splits;
            float separationDistance = 0.5f;

            for (int i = 0; i < splits; i++)
            {
                Quaternion rotation = Quaternion.Euler(0f, 0f, i * angleIncrement);
                Vector2 direction = rotation * transform.right;
                Vector3 spawnPosition = transform.position + (Vector3)(direction * separationDistance); 

                // Instantiate a new projectile
                GameObject dup = Instantiate(baseProjectile, spawnPosition, transform.rotation);
                dup.transform.right = direction;
                dup.GetComponent<Projectile>().Fire(gameObject.GetComponent<ProjectileProperties>());
            }
        }
    }





    public void Despawn(string from)
    {
        despawning = true; // only allow one despawn for one projectile. Stops multiple triggers causing multiple effects being created
        //Debug.Log(from);
        GameObject effect = Instantiate(deathEffect,transform.position, transform.rotation);
        float scale = projProp.getScale();
        effect.transform.localScale = new Vector3(scale, scale, scale); // Scale deatheffect
        if (projProp.isExplosive())
        {
            Explode();
        }
        if (!trail.IsDestroyed())
        {
            trail.transform.parent = null; // decouple trail to allow it to not instantly disappear
            trail.GetComponent<TrailRenderer>().autodestruct = true; // trail destroys when trail reaches end
        }
        // play despawn sound
        gameObject.GetComponent<RandomSound>().PLayClipAt(destroySound, transform.position);
        Destroy(gameObject); // destroy projectile
    }


    /** Fire()
     * Applies the impulse to fire the projectile, depends on the projectiles stats.
     * */
    public void Fire(ProjectileProperties prop)
    {
        this.projProp = prop;
        if (projProp.getSplits() > 0)
        {
            StartCoroutine(SplitCoroutine());
        }
        StartCoroutine(DespawnCoroutine(projProp.getLifetime())); // Start despawn coroutine
        ApplyScale(projProp.getScale()); // Give projectile proper scale
        bouncesLeft = projProp.getBounces(); // Set value for number of bounces remaining
        rb.velocity = transform.right * projProp.getSpeed(); // Give projectile speed
        gameObject.GetComponent<SpriteRenderer>().color = projProp.getSpriteColor();


        // StartCoroutine(SplitCoroutine());
        // // Fire more projectiles with spread angle
        // if (projProp.getShots() > 0)
        // {
        //     int maxSplits = 4;// no more than 4 shots or else bullets collide with themselves
        //     int numberOfProjectiles = Mathf.Min(projProp.getShots() + 1, maxSplits);
        //     float spreadAngle = 35f; 
        //     float separationDistance = 0.5f;
        //     for (int i = 0; i < numberOfProjectiles; i++)
        //     {
        //         float rotationAngle = (i - numberOfProjectiles / 2) * spreadAngle;
        //         Vector3 direction = Quaternion.Euler(0, 0, rotationAngle) * transform.right;
        //         Vector3 spawnPosition = transform.position + direction * separationDistance;

        //         // Instantiate a new projectile at the calculated position
        //         GameObject newProjectile = Instantiate(baseProjectile, spawnPosition, Quaternion.Euler(0, 0, rotationAngle));
        //         Projectile newProjectileScript = newProjectile.GetComponent<Projectile>();
        //         newProjectileScript.projProp = prop;
        //         newProjectileScript.StartCoroutine(newProjectileScript.DespawnCoroutine(newProjectileScript.projProp.getLifetime())); 
        //         newProjectileScript.ApplyScale(newProjectileScript.projProp.getScale()); 
        //         newProjectileScript.bouncesLeft = newProjectileScript.projProp.getBounces(); 
        //         newProjectileScript.rb.velocity = direction * newProjectileScript.projProp.getSpeed(); 
        //         newProjectileScript.gameObject.GetComponent<SpriteRenderer>().color = newProjectileScript.projProp.getSpriteColor();
        //     }
        // }
    }


    private void ApplyScale(float scale)
    {
        // Debug.Log("Scale: " + scale);
        float offset = (scale * 0.2f); // Offset so your projectiles dont hit yourself when scaled up
        transform.position = transform.position + transform.right * offset;
        gameObject.transform.localScale = new Vector3(scale, scale, scale); // Scale projectile
    }

    private void Explode()
    {
        Vector3 offset = new Vector3(50, 50, 0);
        float explosion_scale = projProp.getExplosionScale();
        GameObject expl = Instantiate(explosion, transform.position + offset, transform.rotation); // create explosion offscreen
        expl.transform.localScale = new Vector3(explosion_scale, explosion_scale, explosion_scale); // scale explosion
        expl.GetComponent<Explosion>().SetDamage(projProp.getExplosionDamage()); // Set explosion damage
        expl.transform.position = transform.position; // move explosion back

    }

    private void Burst(Collider2D collision)
    {
        float dupOffset = 0.15f * projProp.getBurstNumber();
        int angle = 180 / (projProp.getBurstNumber() + 1);
        Vector2 pt = collision.ClosestPoint(transform.position);
        Vector2 direction = (pt - new Vector2(transform.position.x, transform.position.y)).normalized;
        direction = Quaternion.Euler(0, 0, +90) * direction;
        for (int i = 0; i < projProp.getBurstNumber(); i++)
        {
            direction = Quaternion.Euler(0, 0, angle) * direction;
            Debug.DrawRay(pt, direction * 1f, Color.red, 1f);
            GameObject dup = Instantiate(baseProjectile, pt + (direction *  dupOffset), transform.rotation);
            dup.transform.right = direction;
            dup.GetComponent<Projectile>().Fire(gameObject.GetComponent<ProjectileProperties>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((collision.CompareTag("Projectile")) && !despawning)
        {
            if (collision.gameObject.GetComponentInParent<Projectile>().projProp.getScale() >= this.projProp.getScale())
            {
                Despawn("Hit Projectile");
            }
            
        } else if ( (collision.CompareTag("Enemy") || collision.CompareTag("Player")) && !despawning)
        {
            if (projProp.isBursting())
            {
                Burst(collision);

            }
            Despawn("Hit Enemy");
            collision.gameObject.GetComponent<Health>().ApplyDamage(projProp.getDamage());
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("Obstacle"))
        {
            this.bouncesLeft = this.bouncesLeft - 1;
            if (this.bouncesLeft < 0)
            {
                if (projProp.isBursting())
                {
                    Burst(collision);
                }
                if (collision.CompareTag("Obstacle"))
                {
                    collision.gameObject.GetComponent<Health>().ApplyDamage(projProp.getDamage());
                }
                Despawn("Hit Wall/Obs");
            }
            else
            {
                if (projProp.isExplosive())
                {
                    // Bounced and is explosive: Create explosion
                    Explode();
                } else
                {
                    // bounced so play bounce sound effect
                    gameObject.GetComponent<RandomSound>().PLayClipAt(bounceSound, transform.position);
                }
            }
            
        }
    }
}

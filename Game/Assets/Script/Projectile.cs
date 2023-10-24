using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Rigidbody2D rb; //projectiles rigidbody

    public float baseSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /** Fire()
     * Applies the impulse to fire the projectile, depends on the projectiles stats.
     * */
    public void Fire()
    {
        rb.velocity = transform.right * baseSpeed;
    }
}

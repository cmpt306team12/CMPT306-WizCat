using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    public ProjectileProperties projectileProperties;
    public GameObject projectile;

    // Shoots the wand
    public void Shoot()
    {
        GameObject newBullet = Instantiate(projectile, transform.GetChild(0).transform.position, transform.GetChild(0).transform.rotation);
        newBullet.GetComponent<Projectile>().Fire(projectileProperties);

    }
}

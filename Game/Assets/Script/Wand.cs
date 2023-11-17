using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Wand : MonoBehaviour
{
    [SerializeField] private GameObject projectile; 
    public ProjectileProperties projectileProperties;
    public GameObject wand;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Shoots the wand
    public void Shoot()
    {
        
        wand.GetComponent<WizWandAnimationControl>().Shoot();
        shootSFX.Play();
        GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Projectile>().Fire(projectileProperties);

    }

    
}

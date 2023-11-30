using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] float explodingBarrelFuseTime = 2.0f;
    [SerializeField] float ExplodingBarrelExplosionScale = 1.0f;
    [SerializeField] float explodingBarrelDamage = 51.0f;
    [SerializeField] float explosionKnockback = 5.0f;
    public GameObject explosion;
    public GameObject fuse;
    private bool isLit;

    private void Start()
    {
        isLit = false;
    }

    public bool IsLit()
    {
        return isLit;
    }

    public void LightFuse()
    {
        // Hit by projectile, start explosion timer
        isLit = true;
        StartCoroutine(ExplodeCoroutine(explodingBarrelFuseTime));
    }

    IEnumerator ExplodeCoroutine(float waitTime)
    {
        // Start fuse animation
        fuse.SetActive(true);
        // wait for specified time
        yield return new WaitForSeconds(waitTime);
        Explode();
        yield return null;
    }

    public void Explode()
    {
        StopAllCoroutines();
        Vector3 offset = new Vector3(50, 50, 0);
        GameObject expl = Instantiate(explosion, transform.position + offset, transform.rotation); // create explosion offscreen
        expl.transform.localScale = new Vector3(ExplodingBarrelExplosionScale, ExplodingBarrelExplosionScale, ExplodingBarrelExplosionScale); // scale explosion
        expl.GetComponent<Explosion>().SetDamage(explodingBarrelDamage); // Set explosion damage
        expl.GetComponent<Explosion>().SetKnockback(explodingBarrelDamage); // Set explosion damage
        expl.transform.position = transform.position; // move explosion back
        Destroy(gameObject);
    }
}

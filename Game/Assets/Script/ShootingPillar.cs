using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPillar : MonoBehaviour
{
    public GameObject wand;
    public GameObject pillar;
    public GameObject firepoint;
    [SerializeField] float shootingDelay = 3.0f;
    public float fireTime;
    private Animator animator;
    public AudioClip shootSoundEffect;
    public LayerMask mask;
    private void Start()
    {
        fireTime = Time.time;
        animator = pillar.GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {


        // cast ray to see if can see player


        if (collision.gameObject.CompareTag("Player") && Time.time > fireTime)
        {
            // cast a ray to see if you can see player
            RaycastHit2D hit = Physics2D.Raycast(firepoint.transform.position, (collision.gameObject.GetComponent<BoxCollider2D>().bounds.center - firepoint.transform.position), 10.0f, ~mask);
            Debug.DrawRay(firepoint.transform.position, (collision.gameObject.GetComponent<BoxCollider2D>().bounds.center - firepoint.transform.position));
            if (hit && hit.transform.CompareTag("Player"))
            {
                fireTime = Time.time + shootingDelay;
                StartCoroutine(FireProjectile());
            }
        }
    }

    private IEnumerator FireProjectile()
    {
        yield return new WaitForSeconds(0.2f); // Delay so pillar doesnt fire the nanosecond it sees you
        gameObject.GetComponent<RandomSound>().PLayClipAt(shootSoundEffect, transform.position);
        // shoot at player
        pillar.GetComponentInChildren<Wand>().Shoot();
        // make shooting sound effect
        gameObject.GetComponent<RandomSound>().PLayClipAt(shootSoundEffect, transform.position);
        animator.SetTrigger("FireTime");
        StopCoroutine(FireProjectile());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("ExitFireZone");
        }
    }
}

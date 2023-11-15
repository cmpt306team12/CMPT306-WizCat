using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPillar : MonoBehaviour
{
    public GameObject wand;
    public GameObject pillar;
    [SerializeField] float shootingDelay = 3.0f;
    public float fireTime;
    private Animator animator;
    public AudioClip shootSoundEffect;
    private void Start()
    {
        fireTime = Time.time;
        animator = pillar.GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > fireTime)
        {
            // shoot at player
            pillar.GetComponentInChildren<Wand>().Shoot();
            // make shooting sound effect
            gameObject.GetComponent<RandomSound>().PLayClipAt(shootSoundEffect, transform.position);
            fireTime = Time.time + shootingDelay;
            animator.SetTrigger("FireTime");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("ExitFireZone");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    
    private Wand _wand;

    [SerializeField] private float firingCooldown;
    [SerializeField] private float moveSpeed;
    private float sightDistance = 25f;

    private bool _canFire = true;
    private bool _canChangeDirection = true;

    //private AudioSource shootSFX;

    private Vector3 _moveDir = Vector3.zero;

    private GameObject _player;
    public Animator animator;
    public AudioClip shootSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.instance.GetPlayer();
        _wand = gameObject.GetComponentInChildren<Wand>();
        //shootSFX = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        transform.Translate(_moveDir * moveSpeed);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        if (_player.transform.position.x > transform.position.x)
        {
            animator.Play("EnemySideIdleR");
        }
        else
        {
            animator.Play("EnemySideIdleL");
        }
        transform.localScale = scale; 


        if (_canChangeDirection)
        {
            StartCoroutine(ChangeDirection());
        }
        if (_canFire)
        {
            StartCoroutine(FireProjectile());
        }

    }

    private IEnumerator ChangeDirection()
    {
        _canChangeDirection = false;
        float movementAngle = Random.Range(0f, 360f);
        _moveDir = Quaternion.AngleAxis(movementAngle,Vector3.forward) * Vector3.up;
        yield return new WaitForSeconds(2);
        _canChangeDirection = true;
    }
    private IEnumerator FireProjectile()
    {
        // cast a ray to see if you can see player
        RaycastHit2D hit = Physics2D.Raycast(_wand.transform.position, (_player.GetComponent<BoxCollider2D>().bounds.center - _wand.transform.position), sightDistance);
        Debug.DrawRay(_wand.transform.position, (_player.gameObject.GetComponent<BoxCollider2D>().bounds.center - _wand.transform.position));
        if (hit && hit.transform.CompareTag("Player"))
        {
            _canFire = false;
            //shootSFX.Play();
            gameObject.GetComponent<RandomSound>().PLayClipAt(shootSoundEffect, transform.position);
            _wand.Shoot();
            yield return new WaitForSeconds(firingCooldown);
            _canFire = true;
        }
    }
}

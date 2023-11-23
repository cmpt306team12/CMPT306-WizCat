using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    
    private Wand _wand;

    [SerializeField] private float firingCooldown;
    [SerializeField] private float moveSpeed;
    private float sightDistance = 25f;

    private bool _canFire = true;
    private bool _canChangeDirection = true;

    public LayerMask mask;

    //private AudioSource shootSFX;

    private Vector3 _moveDir = Vector3.zero;

    private GameObject _player;
    public Animator animator;
    public AudioClip shootSoundEffect;

    public float nextWaypointDistance = 0.2f;
    Path path;
    int currWay = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.instance.GetPlayer();
        _wand = gameObject.GetComponentInChildren<Wand>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 1.0f);
        //StartCoroutine(ChangeDirection());

        //shootSFX = GetComponent<AudioSource>();
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(gameObject.GetComponent<Collider2D>().bounds.center, _player.transform.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currWay = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            Debug.Log("NULL PATH");
            return;
        }
        else if (currWay >= path.vectorPath.Count)
        {
            Debug.Log("END PATH");
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Should move
        _moveDir = (path.vectorPath[currWay] - gameObject.GetComponent<Collider2D>().bounds.center).normalized;
        Debug.DrawLine(transform.position, _moveDir, Color.red);

        float distance = Vector2.Distance(gameObject.GetComponent<Collider2D>().bounds.center, path.vectorPath[currWay]);
        if (distance < nextWaypointDistance)
        {
            currWay++;
        }

        if (!reachedEndOfPath)
        {
            transform.Translate(_moveDir * moveSpeed);
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;

        if (_canFire)
        {
            StartCoroutine(FireProjectile());
        } 

        transform.localScale = scale; 
        animator.SetFloat("Horizontal", _moveDir.x);
        animator.SetFloat("Vertical", _moveDir.y);

    }

    private IEnumerator FireProjectile()
    {
        // cast a ray to see if you can see player
        RaycastHit2D hit = Physics2D.Raycast(_wand.transform.position, (_player.GetComponent<BoxCollider2D>().bounds.center - _wand.transform.position), sightDistance, ~mask);
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

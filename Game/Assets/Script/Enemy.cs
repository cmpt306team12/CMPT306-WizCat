using Pathfinding;
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

    private bool _canFire;
    private bool _canMove;
    private bool _canChangeDirection = true;

    public LayerMask mask;

    private Vector3 _moveDir = Vector3.zero;

    private GameObject _player;
    public Animator animator;
    public AudioClip shootSoundEffect;

    // Pathfinding stuff
    public float nextWaypointDistance = 0.2f;
    Path path;
    int currWay;
    bool reachedEndOfPath;
    Seeker seeker;

    // AI and behaviour stuff
    private bool playerSpotted;
    private float sightDistance = 15.0f;
    private bool stunned;
    public GameObject stun;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.instance.GetPlayer();
        _wand = gameObject.GetComponentInChildren<Wand>();
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();

        _canFire = true;
        _canMove = true;

        reachedEndOfPath = false;
        playerSpotted = false;
        currWay = 0;
        StartCoroutine(ChangeDirection());
    }

    public void Stun(float stunTime)
    {
        stunned = true;
        StartCoroutine(Recover(stunTime));
    }

    public bool IsStunned()
    {
        return stunned;
    }
    IEnumerator Recover(float rt)
    {
        // Display stunned icon
        if (stun == null) yield break;
        else
        {
            stun.SetActive(true);
        }
        yield return new WaitForSeconds(rt);
        if (stun == null) yield break;
        else
        {
            stun.SetActive(false);
        }
        // recover from stun
        stunned = false;
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
        if (playerSpotted && !stunned) // Player spotted, use pathfinding to chase them
        {
            if (path == null) return;
            else if (currWay >= path.vectorPath.Count)
            {
                Debug.Log("END PATH");
                reachedEndOfPath = true;
                return;
            } else reachedEndOfPath = false;

            // Should move
            _moveDir = (path.vectorPath[currWay] - gameObject.GetComponent<Collider2D>().bounds.center).normalized;
            float distance = Vector2.Distance(gameObject.GetComponent<Collider2D>().bounds.center, path.vectorPath[currWay]);
            if (distance < nextWaypointDistance) currWay++;
            if (!reachedEndOfPath) transform.Translate(_moveDir * moveSpeed);
        }
        else if (!stunned) // Player not spotted, move randomly
        {
            // Change direction if able to
            if (_canChangeDirection) StartCoroutine(ChangeDirection());
            // Move 
            transform.Translate(_moveDir * moveSpeed);
        }

        // Start Fire Coroutine if you can fire
        if (_canFire && !stunned)
        {
            StartCoroutine(FireProjectile());
        }

        // Animator stuff
        animator.SetFloat("Horizontal", _moveDir.x);
        animator.SetFloat("Vertical", _moveDir.y);

        // Pathfinding should stop if player is dead
        if (playerSpotted && !_player.GetComponent<Collider2D>().enabled)
        {
            playerSpotted = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 scale = transform.localScale;
        transform.localScale = scale;*/

    }

    void PlayerSpotted()
    {
        playerSpotted = true;
        InvokeRepeating("UpdatePath", 0f, 1.0f);
        return;
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
        RaycastHit2D hit = Physics2D.Raycast(_wand.transform.position, (_player.GetComponent<BoxCollider2D>().bounds.center - _wand.transform.position), sightDistance, ~mask);
        Debug.DrawRay(_wand.transform.position, (_player.gameObject.GetComponent<BoxCollider2D>().bounds.center - _wand.transform.position));
        if (hit && hit.transform.CompareTag("Player"))
        {
            _canFire = false;
            if (!playerSpotted && _player.gameObject.GetComponent<BoxCollider2D>().enabled)
            {
                PlayerSpotted();
            }
            yield return new WaitForSeconds(0.2f); // Delay so enemies dont fire the nanosecond they see you
            
            gameObject.GetComponent<RandomSound>().PLayClipAt(shootSoundEffect, transform.position);
            _wand.Shoot();
            yield return new WaitForSeconds(firingCooldown);
            _canFire = true;
        }
    }
}

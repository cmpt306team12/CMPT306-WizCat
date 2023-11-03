using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] private Wand wand;

    [SerializeField] private float firingCooldown;
    [SerializeField] private float moveSpeed;

    private bool _canFire = true;
    private bool _canChangeDirection = true;

    private Vector3 _moveDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.Translate(_moveDir * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
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
        _canFire = false;
        gameObject.GetComponentInChildren<Wand>().Shoot();
        yield return new WaitForSeconds(firingCooldown);
        _canFire = true;
    }
}

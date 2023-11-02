using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;

    private Rigidbody2D rb;
    private float dashTime;
    public float startDashTime;
    private Vector2 dashDirection = Vector2.zero;
    public float dashImpulseForce = 40f;
    public float dashDrag = 1.0f; 
    // public GameObject dashEffect;


    public float movementSpeed = 5;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.Play("CatWizSideIdleL");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.Play("CatWizSideIdleR");
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }

        // dir.Normalize();
        // GetComponent<Rigidbody2D>().velocity = movementSpeed * dir;

        // Dash on spacebar down
        if (Input.GetKeyDown(KeyCode.Space) && dashTime <= 0)
        {
            dashDirection = dir.normalized;
            dashTime = startDashTime;
            rb.velocity = Vector2.zero; 
            rb.AddForce(dashDirection * dashImpulseForce, ForceMode2D.Impulse);
            // Instantiate(dashEffect, transform.position, Quaternion.identity);

        }

        if (dashTime > 0)
        {
            dashTime -= Time.deltaTime;
        }
        else
        {
            rb.velocity = rb.velocity * (1 - dashDrag * Time.deltaTime);
            if (dir == Vector2.zero)
            {
                rb.velocity = Vector2.zero;
            }

            // If not dashing use walk speed
            if (dashTime <= 0 && dir != Vector2.zero){
                dir.Normalize();
                rb.velocity = movementSpeed * dir;
            }
        }
    }
}

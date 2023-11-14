using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;

    public AudioSource catSFX;
    public AudioClip dashSFX;

    private Rigidbody2D rb;
    private float dashTime;
    public float startDashTime;
    private Vector2 dashDirection = Vector2.zero;
    public float dashImpulseForce = 40f;
    public float dashDrag = 1.0f; 
    // public GameObject dashEffect;
    public bool canDash = false;
    public bool faceUp = false;

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

        //---Strictly Player Movement Controls------------------------------------------------------------------------------------------------------------------------------
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            if (faceUp == false)
            {
                animator.Play("CatWizSideIdleL");
            }
            else
            {
                animator.Play("CatWizBackSideIdleL");
            }
            dir.x = -1;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (faceUp == false)
            {
                animator.Play("CatWizSideIdleR");
            }
            else
            {
                animator.Play("CatWizBackSideIdleR");
            }
            dir.x = 1;

        }

        if (Input.GetKey(KeyCode.W))
        {
            faceUp = true;
            dir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            faceUp = false;
            dir.y = -1;
        }
        
        //---End of Player Movement Controls------------------------------------------------------------------------------------------------------------------------------

        // Dash on spacebar down
        if (Input.GetKeyDown(KeyCode.Space) && dashTime <= 0 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            catSFX.PlayOneShot(dashSFX);
            dashDirection = dir.normalized;
            dashTime = startDashTime;
            rb.velocity = Vector2.zero;
            rb.AddForce(dashDirection * dashImpulseForce, ForceMode2D.Impulse);
            // Instantiate(dashEffect, transform.position, Quaternion.identity);

        }
        if (canDash == true)
        {
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
                if (dashTime <= 0 && dir != Vector2.zero)
                {
                    dir.Normalize();
                    rb.velocity = movementSpeed * dir;
                }
            }

        }

        // Can't dash
        else
        {
            dir.Normalize();
            rb.velocity = movementSpeed * dir;
        }




    }
}

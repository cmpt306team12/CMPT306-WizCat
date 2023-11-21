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
    public bool canDash = false;
    public AudioClip dashSoundEffect;
    public bool faceUp = false;

    public float movementSpeed = 5;
    public Animator animator;
    Vector2 movement;

    private DashIcon dashIconScript;
    public bool onCooldown = false;
    public float dashCooldown = 3f;
    private float currentDashCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        animator = GetComponent<Animator>();
        // grab static data
        canDash = StaticData.canDash;
        dashIconScript = FindObjectOfType<DashIcon>();
    }

    void Update()
    {

        //---Strictly Player Movement Controls------------------------------------------------------------------------------------------------------------------------------
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            
            dir.x = -1;
            animator.SetBool("faceLeft", true);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            dir.x = 1;
            animator.SetBool("faceLeft", false);

        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }

        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Speed", dir.sqrMagnitude);


        //---End of Player Movement Controls------------------------------------------------------------------------------------------------------------------------------

        // Dash on spacebar down
        if (Input.GetKeyDown(KeyCode.Space) && dashTime <= 0 && currentDashCooldown<=0 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            //catSFX.PlayOneShot(dashSFX);
            gameObject.GetComponent<RandomSound>().PLayClipAt(dashSoundEffect, transform.position);
            dashDirection = dir.normalized;
            dashTime = startDashTime;
            currentDashCooldown = dashCooldown;
            onCooldown = true; 
            rb.velocity = Vector2.zero;
            rb.AddForce(dashDirection * dashImpulseForce, ForceMode2D.Impulse);
            // Instantiate(dashEffect, transform.position, Quaternion.identity);

        }
       
        // Time toggle for IU dash Icon 
        if (currentDashCooldown > 0)
        {
            currentDashCooldown -= Time.deltaTime;
            onCooldown = true;
        }
        else
        {
            onCooldown=false;
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

                // If not dashing then walk speed
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
            onCooldown = true;
        }

        if (dashIconScript != null)
        {
            dashIconScript.UpdateDashIcon();
        }
    }
}
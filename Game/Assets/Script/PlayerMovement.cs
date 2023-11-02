using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
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

        dir.Normalize();

        float speed = player.GetComponent<Player>().getSpeed();

        GetComponent<Rigidbody2D>().velocity = speed * dir;
    } 


}

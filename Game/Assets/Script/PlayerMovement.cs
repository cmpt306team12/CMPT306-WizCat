using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            gameObject.transform.localScale = new Vector2(-3, 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            gameObject.transform.localScale = new Vector2(3, 3);
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

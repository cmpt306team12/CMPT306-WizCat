using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPlayerPos : MonoBehaviour
{
    public GameManager gameManager;
    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = gameManager.GetPlayer().GetComponent<BoxCollider2D>().bounds.center;
        Vector2 rotation = playerPos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

    }
}

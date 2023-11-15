using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Camera cam;
    [SerializeField] float cameraSize = 5.0f;
    [SerializeField] float xLimit = 3.0f;
    [SerializeField] float yLimit = 3.0f;
    [SerializeField] float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = cameraSize;
        player = GameManager.instance.GetPlayer().transform;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (player.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -xLimit + player.position.x, xLimit + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -yLimit + player.position.y, yLimit + player.position.y);
        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
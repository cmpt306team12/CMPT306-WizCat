using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Camera cam;
    [SerializeField] float cameraSize = 5.0f;
    [SerializeField] float xLimit = 3.0f;
    [SerializeField] float yLimit = 2.5f;
    [SerializeField] float smoothTime = 0.5f;
    [SerializeField] private float deadzone = 2.5f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = cameraSize;
        player = GameManager.instance.GetPlayer().transform;
    }

    private void Update()
    {
        Vector3 playerPos = player.position;
        Vector3 cameraPos = transform.position;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        float mouseDistFromPlayer = Mathf.Abs(Vector2.Distance(mousePos, playerPos));
        Vector3 targetPos;
        if (mouseDistFromPlayer > deadzone)
        {
            Vector3 deadZoneOffset = deadzone * ((Vector2) mousePos - (Vector2) playerPos).normalized;
            targetPos = (playerPos + mousePos - deadZoneOffset) / 2f;
            targetPos.x = Mathf.Clamp(targetPos.x, -xLimit + playerPos.x, xLimit + playerPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -yLimit + playerPos.y, yLimit + playerPos.y);
            targetPos.z = cameraPos.z;
        }
        else
        {
            targetPos = playerPos;
            targetPos.z = cameraPos.z;
        }
        
        cameraPos = Vector3.SmoothDamp(cameraPos, targetPos, ref velocity, smoothTime);
        transform.position = cameraPos;
    }
}
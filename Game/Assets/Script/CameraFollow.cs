using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraSize = 5.0f;
    public float smoothing = 100.0f;

    private void Start()
    {
        Camera.main.orthographicSize = cameraSize;
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPosition = player.position;
        Vector3 targetPosition = new Vector3(
            (mousePosition.x + playerPosition.x) / 2.4f,
            (mousePosition.y + playerPosition.y) / 2.4f,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime); // smooth
    }
}

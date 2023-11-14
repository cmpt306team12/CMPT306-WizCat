// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     public Transform player;
//     public float cameraSize = 5.0f;
//     public float smoothing = 100.0f;

//     private void Start()
//     {
//         Camera.main.orthographicSize = cameraSize;
//     }

//     private void Update()
//     {
//         Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         Vector3 playerPosition = player.position;
//         Vector3 targetPosition = new Vector3(
//             (mousePosition.x + playerPosition.x) / 2.4f,
//             (mousePosition.y + playerPosition.y) / 2.4f,
//             transform.position.z
//         );

//         transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime); // smooth
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraSize = 5.0f;
    public float smoothing = 100.0f;
    public float maxDistance = 10.0f;
    public float padding = 1.0f; 
    public float maxCameraDistance = 5.0f;

    private void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = cameraSize;
        player = GameManager.instance.GetPlayer().transform;
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPosition = player.position;

        // Calculate the dynamic target position based on the distance
        float distance = Vector3.Distance(mousePosition, playerPosition);
        float dynamicFactor = Mathf.Clamp01(distance / maxDistance); // Adjust this factor as needed

        // If player is not in the camera view, adjust the camera position
        if (!IsPlayerInCameraView())
        {
            Vector3 targetPosition = new Vector3(
                Mathf.Lerp(playerPosition.x, mousePosition.x, dynamicFactor),
                Mathf.Lerp(playerPosition.y, mousePosition.y, dynamicFactor),
                transform.position.z
            );

            // Ensure that the camera doesn't move too far from the player
            float clampedX = Mathf.Clamp(targetPosition.x, playerPosition.x - maxCameraDistance, playerPosition.x + maxCameraDistance);
            float clampedY = Mathf.Clamp(targetPosition.y, playerPosition.y - maxCameraDistance, playerPosition.y + maxCameraDistance);

            transform.position = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, transform.position.z), smoothing * Time.deltaTime);
        }
    }

    private bool IsPlayerInCameraView()
    {
        Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(player.position);
        return playerViewportPosition.x >= 0 + padding && playerViewportPosition.x <= 1 - padding &&
               playerViewportPosition.y >= 0 + padding && playerViewportPosition.y <= 1 - padding;
    }
}
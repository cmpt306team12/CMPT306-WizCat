using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraSize = 5.0f;
    public float smoothing = 5.0f;
    public float maxCameraDistance = 5.0f; 
    public float verticalPadding = 0.5f; 

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
        Vector3 targetPosition = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);

        float clampedX = Mathf.Clamp(mousePosition.x, playerPosition.x - maxCameraDistance, playerPosition.x + maxCameraDistance);
        float clampedY = Mathf.Clamp(mousePosition.y, playerPosition.y - maxCameraDistance * verticalPadding, playerPosition.y + maxCameraDistance * verticalPadding);

        // Set the new camera position
        transform.position = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, transform.position.z), smoothing * Time.deltaTime);
    }
}
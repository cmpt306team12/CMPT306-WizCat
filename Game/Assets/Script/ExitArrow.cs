using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExitArrow : MonoBehaviour
{
    public Vector2 targetCoordinates = new Vector2 (0, 0);
    public float distance = 6f;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = CalculateTargetPosition();

        gameObject.transform.position = targetPosition;

        AvoidPlayerOverlap();

        RotateArrow();
    }

    Vector3 CalculateTargetPosition()
    {
        // Calculate the direction vector from the player to the target coordinates
        Vector3 directionToTarget = (new Vector3(targetCoordinates.x, targetCoordinates.y, 0f) - transform.position).normalized;
        // Calculate the target position based on the direction and distance
        Vector3 targetPosition = transform.position + directionToTarget * distance;

        return targetPosition;
    }

    void AvoidPlayerOverlap()
    {
        // Calculate the vector from the player to the exitArrow
        Vector3 vectorToArrow = gameObject.transform.position - transform.position;

        // Ensure the exitArrow is always at the specified distance from the player
        if (vectorToArrow.magnitude < distance)
        {
            // Move the exitArrow to the edge of the allowed distance
            gameObject.transform.position = transform.position + vectorToArrow.normalized * distance;
        }
    }

    void RotateArrow()
    {
        // Calculate the direction vector from the player to the exitArrow
        Vector3 directionToArrow = (gameObject.transform.position - transform.position).normalized;

        // Calculate the rotation angle to look away from the player
        float angle = Mathf.Atan2(directionToArrow.y, directionToArrow.x) * Mathf.Rad2Deg;

        // Rotate the exitArrow to look away from the player and then rotate 180 degrees
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
    }
}

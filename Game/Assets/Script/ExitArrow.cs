using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExitArrow : MonoBehaviour
{
    public Vector2 targetCoordinates = new Vector2(32f, 42.5f);// Target coordinates that the arrow should point towards
    public float offsetDistance = 2.5f; // Offset distance from the player

    void Update()
    {
        // Assuming the parent of the ExitArrow is the player
        Transform playerTransform = transform.parent;

        // Calculate the direction from the player to the target coordinates
        Vector2 directionToTarget = targetCoordinates - (Vector2)playerTransform.position;
        directionToTarget.Normalize(); // Normalize the direction vector

        // Calculate the position of the ExitArrow based on the offset distance
        Vector2 arrowPosition = (Vector2)playerTransform.position + directionToTarget * offsetDistance;

        // Set the ExitArrow's local position
        transform.localPosition = arrowPosition - (Vector2)playerTransform.position;

        // Calculate the rotation angle to make the arrow point towards the target coordinates
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        angle -= 90f;

        // Set the ExitArrow's local rotation
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}

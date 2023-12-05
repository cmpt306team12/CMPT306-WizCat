using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    [SerializeField] float rotationAmount = 1.0f;
    [SerializeField] float rotationx;
    [SerializeField] float rotationy;
    [SerializeField] float rotationz;
    Vector3 direction;
    private void Start()
    {
        direction = new Vector3 (rotationx, rotationy, rotationz);
    }
    private void FixedUpdate()
    {
        // Rotate Gameobject around axis
        gameObject.transform.Rotate(direction, rotationAmount);
    }
}

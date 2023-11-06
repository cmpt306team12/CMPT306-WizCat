using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanOrbit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                OrbitProjectiles.canOrbit = true;
            }
        }
    }
}

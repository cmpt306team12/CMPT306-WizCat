using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBite : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Bite.canBite = true;
        }
    }
}
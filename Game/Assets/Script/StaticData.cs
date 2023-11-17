using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    // initial values Set here
    // fetches current scene values before swapping scenes
    // set in GameManager.cs
    public static int coins = 0;
    public static int score = 0;
    // set in Health.cs
    public static float maxHealth = 100;
    public static float currentHealth = 100;
    // set in Player.cs
    public static int[] perks = new int[9];
    // set in PlayerMovement.cs
    public static bool canDash = false;


    static public void Reset()
    {
        coins = 0;
        score = 0;
        maxHealth = 100;
        currentHealth = 100;
        perks = new int[9];
        canDash = false;
        Bite.canBite = false;
        OrbitProjectiles.canOrbit = false;
    }
}

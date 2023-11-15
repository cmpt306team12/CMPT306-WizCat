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
    public static float maxHealth = 1000;
    public static float currentHealth = 1000;
    // set in Player.cs
    public static int[] perks = new int[8];
    // set in PlayerMovement.cs
    public static bool canDash = false;
}

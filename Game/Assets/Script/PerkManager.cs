using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perks : MonoBehaviour
{
    public ProjectileProperties projectileProperties;

    /* Perk mods array:
    * 0: Bounces
    * 1: Speed (+/-)
    * 2: Lifetime (+/-)
    * 3: Damage (+/-)
    * 4; Explosive
    * 5: Size(+/-)
    * 6: Burst
    * 7: Non-modify
    * 8: Split
    * 9: Homing Shots
    * 10: Boomerang Shots
    * 11: Wiggle Shots
    * */
    int[] perks = new int[12];

    public void AddPerk(int i)
    {
        int change = 1;
        int index = 0;
        switch (i)
        {
            case 0: case 1:// bounce+ or speed+
                index = i;
                change = 1;
                break;
            case 2: //speed-
                index = 1;
                change = -1;
                break;
            case 3: //time+
                index = 2;
                change = 1;
                break;
            case 4: //time-
                index = 2;
                change = -1;
                break;
            case 5: //damage+
                index = 3;
                change = 1;
                break;
            case 6: //damage-
                index = 3;
                change = -1;
                break;
            case 7: //explosive+
                index = 4;
                change = 1;
                break;
            case 8: //size+
                index = 5;
                change = 1;
                break;
            case 9: //size-
                index = 5;
                change = -1;
                break;
            case 10: //burst
                index = 6;
                change = 1;
                break;
            case 11: //bite
                index = 7;
                break;
            case 12: //dash
                index = 7;
                break;
            case 13: //orbit
                index = 7;
                break;
            case 14: //splits+
                index = 8;
                change = 1;
                break;
            case 15: //homing+
                index = 9;
                change = 1;
                break;
            case 16: //boomerang+
                index = 10;
                change = 1;
                break;
            case 17: //wiggle+
                index = 11;
                change = 1;
                break;
            default:
                break;
        }
        try
        {
            perks[index] = perks[index] + change;
        } catch (Exception)
        {
            Debug.LogError("Perk index out of bounds", this);
        }
        projectileProperties.ApplyPerks(perks);
    }

    public int[] GetPerks()
    {
        return perks;
    }

    public void SetPerks(int[] perks) 
    {
        this.perks = perks;
    }

    public static List<Tuple<int, int>> GetValidEnemyPerks()
    {
        return new List<Tuple<int, int>>{
            new Tuple<int, int>(0, 1), // bounce+
            new Tuple<int, int>(1, 2), // speed+
            new Tuple<int, int>(3, 1), // time+
            new Tuple<int, int>(5, 2), // damage+
            new Tuple<int, int>(7, 2), // explosive+
            new Tuple<int, int>(8, 3), // size+
            new Tuple<int, int>(10, 3), // burst+
            new Tuple<int, int>(14, 2), // splits+
    };
}
}

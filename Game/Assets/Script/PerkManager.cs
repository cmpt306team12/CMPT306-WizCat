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
    * */
    int[] perks = new int[10];

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
}

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
    * */
    int[] perks = new int[8];

    public void AddPerk(int i)
    {
        int change = 1;
        int index = 0;
        switch (i)
        {
            case 0: case 1:// bounce+ or speed+
                index = i;
                break;
            case 2: //speed-
                index = 1;
                change = -1;
                break;
            case 3: //time+
                index = 2;
                break;
            case 4: //time-
                index = 2;
                change = -1;
                break;
            case 5: //damage+
                index = 3;
                break;
            case 6: //damage-
                index = 3;
                change = -1;
                break;
            case 7: //explosive+
                index = 4;
                break;
            case 8: //size+
                index = 5;
                break;
            case 9: //size-
                index = 5;
                change = -1;
                break;
            case 10: //Burst
                index = 6;
                break;
            case 11: //empty perk
                index = 7;
                break;
            default:
                break;
        }
        try
        {
            perks[index] = perks[index] + change;
        } catch (Exception e)
        {
            Debug.LogError(e.Message, this);
        }
        projectileProperties.ApplyPerks(perks);
    }

    public int[] GetPerks()
    {
        return perks;
    }
}

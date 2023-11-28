using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignedPerks : MonoBehaviour
{
    public ProjectileProperties projProp;
    public int NumberOfBounce = 0;
    public int NumberOfSpeed = 0;
    public int NumberOfLifetime = 0;
    public int NumberOfDamage = 0;
    public int NumberOfExplosive = 0;
    public int NumberOfSize = 0;
    public int NumberOfBurst = 0;
    public int NumberOfSplit = 0;
    public int NumberOfHoming = 0;
    private int[] perks;
    // Start is called before the first frame update
    private void Awake()
    {
        perks = new int[10]; // One less than player because enemies cant Dash, Orbit or Bite which use index 7 (non-modify projectiles)
        perks[0] = NumberOfBounce;
        perks[1] = NumberOfSpeed;
        perks[2] = NumberOfLifetime;
        perks[3] = NumberOfDamage;
        perks[4] = NumberOfExplosive;
        perks[5] = NumberOfSize;
        perks[6] = NumberOfBurst;
        perks[7] = 0; // Reserved for PLayer perks, doesn't modifiy projectiles.
        perks[8] = NumberOfSplit;
        perks[9] = NumberOfHoming;
        projProp.ApplyPerks(perks);
    }
}

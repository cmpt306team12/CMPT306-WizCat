using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    [SerializeField] int perkID;

    public int GetPerkID()
    {
        return perkID;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}

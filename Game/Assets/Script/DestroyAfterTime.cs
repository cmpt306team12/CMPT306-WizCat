using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDespawn : MonoBehaviour
{
    public float lifetime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, lifetime);
    }

}

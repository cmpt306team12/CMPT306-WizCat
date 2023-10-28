using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{ 
    [SerializeField] private GameObject projectile;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            projectile.GetComponent<Projectile>().Fire();
            Instantiate(projectile, transform.position, transform.rotation);
            
        }


    }
}

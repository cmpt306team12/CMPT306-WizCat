using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (StaticData.crown)
        {
            gameObject.SetActive(true);
        }
    }


}

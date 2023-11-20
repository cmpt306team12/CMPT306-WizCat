using UnityEngine;
using UnityEngine.UI;

public class OrbitIcon : MonoBehaviour
{
    public Image iconImage;

    void Update()
    {
        if (OrbitProjectiles.canOrbit)
        {
            if (!OrbitProjectiles.onCooldown)
            {
                iconImage.color = Color.white;
            }
            else
            {
                iconImage.color = Color.grey;
            }
        }
        else
        {
            iconImage.color = Color.grey;
        }
    }
}
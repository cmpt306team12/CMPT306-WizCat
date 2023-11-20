using UnityEngine;
using UnityEngine.UI;

public class DashIcon : MonoBehaviour
{
    public Image iconImage;
    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void UpdateDashIcon()
    {
        if (playerMovement.canDash)
        {
            if (!playerMovement.onCooldown)
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
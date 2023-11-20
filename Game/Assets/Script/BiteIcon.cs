using UnityEngine;
using UnityEngine.UI;

public class BiteIcon : MonoBehaviour
{
    public Image iconImage;
    public Bite bite;

    void Start()
    {
        bite = FindObjectOfType<Bite>();
    }

    void Update()
    {
        if (Bite.canBite)
        {
            if (!bite.onCooldown)
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

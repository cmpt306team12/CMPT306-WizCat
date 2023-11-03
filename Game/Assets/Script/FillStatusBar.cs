using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    public Health playerHealth;
    public Image fillImage;
    private Slider slider;
    private Text healthText;
    
    void Awake()
    {
        // When the game is loading, it will search for the slider component
        // (the health bar), and make a reference to it
        slider = GetComponent<Slider>();
        healthText = GetComponentInChildren<Text>();
        healthText.text = playerHealth + " / " + playerHealth.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // This code helps to remove the little bit of fill left when slider value is at 0
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        // Conversely, this code enables the slider if it is greater than the min value of 0
        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }
        
        AdjustHealthBar();
    }

    void AdjustHealthBar()
    {
        float fillValue = (playerHealth.currentHealth / playerHealth.maxHealth);
        slider.value = fillValue;
        
        // Update the healthText to display the current health
        healthText.text = playerHealth.currentHealth + " / " + playerHealth.maxHealth;
    }
}

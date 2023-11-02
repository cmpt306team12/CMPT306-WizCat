using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Enemy enemyHealth;
    [SerializeField] Slider slider;
    public Image fillImage;

    private void Awake()
    {
        slider = GetComponent<Slider>();
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
        
        UpdateHealthBar();
    }
    
    public void UpdateHealthBar()
    {
        float fillValue = (enemyHealth.currentHealth / enemyHealth.maxHealth);
        slider.value = fillValue;
    }
}

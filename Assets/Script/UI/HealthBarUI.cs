using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private Image fillImage;
    public Player player;

    void Start()
    {
        healthSlider = GetComponent<Slider>();
        fillImage = healthSlider.fillRect.GetComponent<Image>();

        if (player != null)
        {
            healthSlider.maxValue = player.MaxHealth;
            healthSlider.value = player.CurrentHealth;
        }
    }


    void Update()
    {
        if (player != null)
        {
            healthSlider.value = player.CurrentHealth;
            fillImage.enabled = player.CurrentHealth > 0;
        }
    }

}

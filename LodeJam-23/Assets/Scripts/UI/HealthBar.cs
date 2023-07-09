using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : BaseHealthBar
{
    public Slider slider;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    
    private void SetHealth(float health)
    {
        slider.value = health;
    }
    
    public override void OnPlayerHealthChange()
    {
        SetHealth(GameManager.Instance.GetPlayerEntity().GetCurrentHealth());
    }
}

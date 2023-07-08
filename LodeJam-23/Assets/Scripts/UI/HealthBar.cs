using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerHealthbar
{
    public void OnPlayerHealthChange();
}

public class HealthBar : MonoBehaviour, IPlayerHealthbar
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
    
    public void OnPlayerHealthChange()
    {
        SetHealth(GameManager.Instance.GetPlayerEntity().GetCurrentHealth());
    }
}

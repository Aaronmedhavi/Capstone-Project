

using System;
using UnityEngine;
using UnityEngine.UI;

public class AlertSlider : MonoBehaviour
{
    [SerializeField] Slider circularSlider;
    [SerializeField] float AlertMaxValue;

    float CurrentAlertValue;
    public float AlertValue
    {
        get => CurrentAlertValue;
        set
        {
            CurrentAlertValue = Mathf.Clamp(value, value, AlertMaxValue);
            if (CurrentAlertValue < 0 || CurrentAlertValue == AlertMaxValue) OnRelease(this);
            else circularSlider.@value = CurrentAlertValue / AlertMaxValue;
        }
    }
    public Action<AlertSlider> OnRelease;
    public void Alert(bool isAlert, Enemy.ViewSettings settings)
    {
        float AlertGained = settings.AlertGains;
        AlertValue += (isAlert ? AlertGained : -AlertGained) * Time.deltaTime;
    }
}

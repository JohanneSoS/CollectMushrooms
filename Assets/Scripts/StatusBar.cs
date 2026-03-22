using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider exhaustionSlider;

    void Awake()
    {
        GlobalEventManager.UpdateHealthBar.AddListener(setHealth);
        GlobalEventManager.UpdateHungerBar.AddListener(setHunger);
        GlobalEventManager.UpdateExhaustionBar.AddListener(setExhaustion);
    }

    private void setHealth(int newValue)
    {
        healthSlider.value = newValue;
    }

    private void setHunger(int newValue)
    {
        hungerSlider.value = newValue;
    }

    private void setExhaustion(int newValue)
    {
        exhaustionSlider.value = newValue;
    }
}

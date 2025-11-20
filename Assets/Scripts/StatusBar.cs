using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    void Awake()
    {
        EventManager.UpdateHealthBar.AddListener(setHealth);
    }

    private void setHealth(int newValue)
    {
        healthSlider.value = newValue;
    }
}

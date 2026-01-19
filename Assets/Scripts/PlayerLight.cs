using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLight : MonoBehaviour
{
    private Light2D charLight;
    
    [SerializeField] private float lightIntensityNight;
    [SerializeField] private float lightIntensityEvening;
    [SerializeField] private float sniffLightRadius;
    
    private bool changeLightIntensity = false;
    private bool isNight = false;
    private bool isEvening = false;
    private float currentLightRadius;

    void Awake()
    {
        charLight = GetComponent<Light2D>();
        EventManager.OnDayStart.AddListener(DayStart);
        EventManager.OnEveningStart.AddListener(EveningStart);
        EventManager.OnNightStart.AddListener(NightStart);
        EventManager.OnSniffing.AddListener(SniffStart);
        EventManager.OnSniffingEnd.AddListener(SniffStop);
    }

    void FixedUpdate()
    {
        UpdateLightCircle();
    }
    private void UpdateLightCircle()
    {
        var t = Time.deltaTime * 1f;
        if (changeLightIntensity)
        {
            if (isNight)
            {
                if (charLight.intensity != lightIntensityNight)
                {
                    charLight.intensity = Mathf.Lerp(charLight.intensity, lightIntensityNight, t);
                }
                else
                {
                    changeLightIntensity = false;
                }
            }
            else if (!isNight)
            {
                if (charLight.intensity != lightIntensityEvening)
                {
                    charLight.intensity = Mathf.Lerp(charLight.intensity, lightIntensityEvening, t);
                }
                else
                {
                    changeLightIntensity = false;
                }
            }
        }
    }
    
    private void DayStart()
    {
        charLight.enabled = false;
        charLight.intensity = 0f;
    }

    private void EveningStart()
    {
        changeLightIntensity = true;
        charLight.enabled = true;
        isNight = false;
    }

    private void NightStart()
    {
        changeLightIntensity = true;
        charLight.enabled = true;
        isNight = true;
        print("Player realises Night started");
    }

    private void SniffStart()
    {
        currentLightRadius = charLight.pointLightOuterRadius;
        if (charLight.enabled == true)
        {
            charLight.pointLightOuterRadius = sniffLightRadius;
        }
    }

    private void SniffStop()
    {
        if (charLight.enabled == true)
        {
            charLight.pointLightOuterRadius = currentLightRadius;
        }
    }
}

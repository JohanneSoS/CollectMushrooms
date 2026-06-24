using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClockManager : MonoBehaviour
{
    [SerializeField] public Volume postProcessVolume;
    [SerializeField] public Light2D globalLight;
    [SerializeField] public TextMeshProUGUI timeDisplay;
    
    public float tick;
    public float defaultTick;
    public float seconds;
    public float minutes;
    public float hours;
    public float days = 1;

    [SerializeField] private float dayStartHour;
    [SerializeField] private float noonStartHour;
    [SerializeField] private float eveningStartHour;
    [SerializeField] private float nightStartHour;
    
    public GameObject PlayerLight;
    public PlayerStats playerStats;
    [SerializeField] private float dayLightIntensity;
    [SerializeField] private float nightLightIntensity;
    [SerializeField] private float eveningLightIntensity;
    [SerializeField] private float dayTimeShiftDuration;

    [SerializeField] private int hungerPerHour;
    [SerializeField] private int exhaustionPerHour;
    [SerializeField] private int damagePerHour;

    [SerializeField] private bool isNight;
    private bool isEvening;
    private bool dayTimeShifting;

    void Awake()
    {
        GlobalEventManager.OnSkipToDay.AddListener(SkipToDay);
        GlobalEventManager.ToggleUI.AddListener(OnUIToggle);
    }
    void Start()
    {
        tick = defaultTick;
    }
    
    private void FixedUpdate()
    {
        CalcTime();
        DisplayTime();
    }

    public void CalcTime()
    {
        seconds += Time.fixedDeltaTime * tick;

        if (seconds >= 60)
        {
            seconds = 0;
            minutes += 1;
        }

        if (minutes >= 60)
        {
            minutes = 0;
            hours += 1;
            GlobalEventManager.ApplyHunger.Invoke(hungerPerHour);
            GlobalEventManager.ApplyExhaustion.Invoke(exhaustionPerHour);
            if (playerStats.currentHunger <= 0)
            {
                GlobalEventManager.ApplyDamage.Invoke(damagePerHour);
                playerStats.currentHunger = 0;
            }
            if (playerStats.currentExhaustion <= 0)
            {
                GlobalEventManager.ApplyDamage.Invoke(damagePerHour);
                playerStats.currentExhaustion = 0;
            }
        }

        if (hours >= 24)
        {
            hours = 0;
            days += 1;
        }

        ControlPostProcessing();
    }

    void ControlPostProcessing()
    {
        var t = Time.deltaTime * dayTimeShiftDuration;
        //EveningStart
        if(hours >= eveningStartHour && hours < (eveningStartHour+1))
        {
            if (dayTimeShifting)
            {
                if (globalLight.intensity != eveningLightIntensity) 
                {
                    globalLight.intensity = Mathf.Lerp(globalLight.intensity, eveningLightIntensity, t);
                }
                else
                {
                    dayTimeShifting = false;
                }
            }
            if (isEvening == false && isNight == false)
            {
                if (minutes > 1)
                {
                    dayTimeShifting = true;
                    GlobalEventManager.OnEveningStart.Invoke();
                    isEvening = true;
                }
            }
        }
        //NightStart
        if(hours >= nightStartHour && hours < (nightStartHour+1))
        {
            if (dayTimeShifting)
            {
                if (globalLight.intensity != nightLightIntensity) 
                {
                    globalLight.intensity = Mathf.Lerp(globalLight.intensity, nightLightIntensity, t);
                }
                else
                {
                    dayTimeShifting = false;
                }
            }
            if (isNight == false && isEvening)
            {
                if (minutes > 1)
                {
                    dayTimeShifting = true;
                    GlobalEventManager.OnNightStart.Invoke();
                    isNight = true;
                    isEvening = false;
                }
            }
        }
        
        //Day Start
        if (hours >= dayStartHour && hours < (dayStartHour + 1))
        {
            if (dayTimeShifting)
            {
                if (globalLight.intensity != dayLightIntensity) 
                {
                    globalLight.intensity = Mathf.Lerp(globalLight.intensity, dayLightIntensity, t);
                }
                else
                {
                    dayTimeShifting = false;
                }
            }
            if (isNight)
            {
                if (minutes > 1)
                {
                    dayTimeShifting = true;
                    GlobalEventManager.OnDayStart.Invoke();
                    isNight = false;
                    isEvening = false;
                }
            }
        }
    }

    void DisplayTime()
    {
        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

    void TurnOnPlayerLight()
    {
        PlayerLight.SetActive(true);
    }

    void TurnOffPlayerLight()
    {
        PlayerLight.SetActive(false);
    }

    void SkipToDay()
    {
        if (isNight == false)
        {
            isNight = true;
        }
        if (hours >= 18)
        {
            days = days + 1;
            GlobalEventManager.ResetExhaustion.Invoke();
            GlobalEventManager.ApplyHeal.Invoke(50);
        }
        else
        {
            GlobalEventManager.ApplyExhaustion.Invoke(-50);
            GlobalEventManager.ApplyHeal.Invoke(25);
        }
        StartDay();
    }

    void StartDay()
    {
        hours = 5;
        minutes = 59;
        seconds = 59;
    }

    void OnUIToggle(bool uiState)
    {
        if (uiState)
        {
            tick = 0;
            Time.timeScale = 0f;
        }
        else if (!uiState)
        {
            tick = defaultTick;
            Time.timeScale = 1f;
        }
    }

    void Respawn()
    {
        if (hours <= 6)
        {
            days = days - 1;
        }
        StartDay();
    }
}

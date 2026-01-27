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
    private float defaultTick;
    public float seconds;
    public float minutes;
    public float hours;
    public float days = 1;

    [SerializeField] private float dayStartHour;
    [SerializeField] private float noonStartHour;
    [SerializeField] private float eveningStartHour;
    [SerializeField] private float nightStartHour;
    
    public GameObject PlayerLight;
    [SerializeField] private float dayLightIntensity;
    [SerializeField] private float nightLightIntensity;
    [SerializeField] private float eveningLightIntensity;
    [SerializeField] private float dayTimeShiftDuration;

    [SerializeField] private int hungerPerHour;
    [SerializeField] private int exhaustionPerHour;

    [SerializeField] private bool isNight;
    private bool isEvening;
    private bool dayTimeShifting;

    void Awake()
    {
        EventManager.OnSkipToDay.AddListener(SkipToDay);
        EventManager.ToggleUI.AddListener(OnUIToggle);
    }
    void Start()
    {
        //postProcessVolume.weight = 1;
        defaultTick = tick;
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
            EventManager.ApplyHunger.Invoke(hungerPerHour);
            EventManager.ApplyExhaustion.Invoke(exhaustionPerHour);
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
            //postProcessVolume.weight = 0.6f * ((float)minutes / 60);
            //globalLight.intensity = eveningLightIntensity - (float)minutes / 60;
            
            if (isEvening == false && isNight == false)
            {
                if (minutes > 1)
                {
                    dayTimeShifting = true;
                    EventManager.OnEveningStart.Invoke();
                    print("Signal of Starting Evening reached");
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
            //postProcessVolume.weight = 0.6f + ((float)minutes / 60);
            //globalLight.intensity = dayLightIntensity - (float)minutes / 60;
            
            if (isNight == false && isEvening)
            {
                if (minutes > 1)
                {
                    dayTimeShifting = true;
                    EventManager.OnNightStart.Invoke();
                    print("Signal of Starting Night reached");
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
            //postProcessVolume.weight = 1 - (float)minutes / 60;
            //globalLight.intensity = nightLightIntensity + ((float)minutes / 60);
            
            if (isNight)
            {
                if (minutes > 1)
                {
                    dayTimeShifting = true;
                    EventManager.OnDayStart.Invoke();
                    isNight = false;
                    isEvening = false;
                    print("Signal of Starting Day reached");
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
            EventManager.ResetExhaustion.Invoke();
            EventManager.ApplyHeal.Invoke(50);
        }
        else
        {
            EventManager.ApplyExhaustion.Invoke(-50);
            EventManager.ApplyHeal.Invoke(25);
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

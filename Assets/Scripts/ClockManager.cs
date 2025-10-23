using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ClockManager : MonoBehaviour
{
    [SerializeField] public Volume postProcessVolume;
    [SerializeField] public TextMeshProUGUI timeDisplay;
    
    public float tick;
    public float seconds;
    public float minutes;
    public float hours;
    public float days = 1;

    [SerializeField] private float dayStartHour;
    [SerializeField] private float noonStartHour;
    [SerializeField] private float eveningStartHour;
    [SerializeField] private float nightStartHour;
    
    public GameObject PlayerLight;

    private bool isNight;
    
    void Start()
    {
        postProcessVolume.weight = 1;
        isNight = true;
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
        //EveningStart
        if(hours >= eveningStartHour && hours < (eveningStartHour+1))
        {
            postProcessVolume.weight = 0.6f * ((float)minutes / 60);
            
            if (isNight == false)
            {
                if (minutes > 45)
                {
                    EventManager.OnNightStart.Invoke();
                    print("Signal of Starting Night reached");
                    isNight = true;
                }
            }
        }
        
        //NightStart
        if(hours >= nightStartHour && hours < (nightStartHour+1))
        {
            postProcessVolume.weight = 0.6f + ((float)minutes / 60);
        }
        
        //Day Start
        if (hours >= dayStartHour && hours < (dayStartHour + 1))
        {
            postProcessVolume.weight = 1 - (float)minutes / 60;
            
            if (isNight)
            {
                if (minutes > 45)
                {
                    EventManager.OnDayStart.Invoke();
                    isNight = false;
                    print("Signal of Starting Day reached");
                }
            }
        }
    }

    void DisplayTime()
    {
        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, minutes);
    } 
}

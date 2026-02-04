using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Unity.VisualScripting;

public class AudioZoneManager : MonoBehaviour
{

    public string currentCol;
    
    private AudioZone river = AudioZone.Outside;
    private AudioZone npc = AudioZone.Outside;
    private AudioZone wolf = AudioZone.Outside;

    private float oldRiverValue = 0f;
    private float newRiverValue = 0f;

    private float oldRacoonValue = 0f;
    private float newRacoonValue = 0f;
    

    void Awake()
    {
        EventManager.EnterAudioZone.AddListener(OnEnterZone);
        EventManager.ExitAudioZone.AddListener(OnExitZone);
        EventManager.OnCompleteQuest.AddListener(OnCompleteQuest);
    }

    void Update()
    {

        CheckRiverZone();
        //CheckWolfZone();
        CheckNPCZone();

        if (oldRiverValue != newRiverValue)
        {
            //FmodEvents.instance._ambienceInstance.setParameterByName("River", (Mathf.Lerp(oldRiverValue, newRiverValue, time)));
            StartCoroutine(WaitForLerpRiver(oldRiverValue, newRiverValue, "River"));
        }


        if (oldRacoonValue != newRacoonValue)
        {
            //FmodEvents.instance._musicInstance.setParameterByName("Racoon", (Mathf.Lerp(oldRacoonValue, newRacoonValue, 1)));
            StartCoroutine(WaitForLerpRacoon(oldRacoonValue, newRacoonValue, "Racoon"));
        }
    }

    private IEnumerator WaitForLerpRiver(float oldValue, float newValue, string parameter)
    {
        float time = 0;
        
        while (time < 1)
        {
            FmodEvents.instance._musicInstance.setParameterByName(parameter, (Mathf.Lerp(oldRiverValue, newRiverValue, time / 1)));
            //oldValue = Mathf.Lerp(oldValue, newValue, time / 1);
            time += Time.deltaTime;
            yield return null;
        }
        oldRiverValue = newRiverValue;
    }

    private IEnumerator WaitForLerpRacoon(float oldValue, float newValue, string parameter)
    {
        float time = 0;
        
        while (time < 1)
        {
            FmodEvents.instance._musicInstance.setParameterByName(parameter, (Mathf.Lerp(oldRacoonValue, newRacoonValue, time / 1)));
            //oldValue = Mathf.Lerp(oldValue, newValue, time / 1);
            time += Time.deltaTime;
            yield return null;
        }
        oldRacoonValue = newRacoonValue;
    }

    void CheckRiverZone()
    {
        //ToDo: Create New Better Zones
        switch (river)
        {
            case AudioZone.Outside:
                newRiverValue = 0f;
                break;
            case AudioZone.Furthest:
                newRiverValue = 0.2f;
                break;
            case AudioZone.Far:
                newRiverValue = 0.4f;
                break;
            case AudioZone.Mid:
                newRiverValue = 0.6f;
                break;
            case AudioZone.Close:
                newRiverValue = 0.8f;
                break;
            case AudioZone.Closest:
                newRiverValue = 1;
                break;
        }
    }

    /*void CheckWolfZone()
    {
        if (!wolfZoneClose && !wolfZoneFar)
        {
            FmodEvents.instance._musicInstance.setParameterByName("Wolf", 0f);
        }
        else if (wolfZoneClose)
        {
            FmodEvents.instance._musicInstance.setParameterByName("Wolf", 1f);
        }
        else if (wolfZoneFar)
        {
            FmodEvents.instance._musicInstance.setParameterByName("Wolf", 0.5f);
        }
    }*/

    void CheckNPCZone()
    {
        //ToDo: Create New Better Zones
        switch (npc)
        {
            case AudioZone.Outside:
                newRacoonValue = 0f;
                break;
            case AudioZone.Furthest:
                break;
            case AudioZone.Far:
                newRacoonValue = 1f;
                break;
            case AudioZone.Mid:
                break;
            case AudioZone.Close:
                newRacoonValue = 2f;
                break;
            case AudioZone.Closest:
                newRacoonValue = 3f;
                break;
        }
    }
    void OnEnterZone(AudioZone zone, ZoneOrigin origin)
    {
        switch (origin)
        {
            case ZoneOrigin.River:
                river = zone;
                break;
            case ZoneOrigin.Racoon:
                npc = zone;
                break;
        }
    }

    void OnExitZone(AudioZone zone, ZoneOrigin origin)
    {
        switch (origin)
        {
            case ZoneOrigin.River:
                river = (zone - 1);
                break;
            case ZoneOrigin.Racoon:
                npc = (zone - 1);
                break;
        }
    }

    void OnCompleteQuest(int questCount)
    {
        npc = AudioZone.Outside;
    }
}

public enum AudioZone 
{
    Outside,
    Furthest,
    Far,
    Mid,
    Close,
    Closest
}

public enum ZoneOrigin
{
    River,
    Wolf,
    Racoon,
    Beaver,
    Boar
}
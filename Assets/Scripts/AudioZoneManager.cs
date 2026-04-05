using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Unity.VisualScripting;

public class AudioZoneManager : MonoBehaviour
{

    public string currentCol;
    
    private AudioZone river = AudioZone.Outside;
    private AudioZone racoon = AudioZone.Outside;
    private AudioZone beaver = AudioZone.Outside;
    private AudioZone jay = AudioZone.Outside;
    private AudioZone boar = AudioZone.Outside;
    private AudioZone wolf = AudioZone.Outside;

    /*private float oldRiverValue = 0f;
    private float newRiverValue = 0f;

    private float oldRacoonValue = 0f;
    private float newRacoonValue = 0f;
    private float oldBeaverValue = 0f;
    private float newBeaverValue = 0f;
    private float oldBoarValue = 0f;
    private float newBoarValue = 0f;
    private float oldWolfValue = 0f;*/

    Dictionary<ZoneOrigin, AudioZone> npcs = new Dictionary<ZoneOrigin, AudioZone>();
    Dictionary<ZoneOrigin, float> oldValues = new Dictionary<ZoneOrigin, float>();
    Dictionary<ZoneOrigin, float> newValues = new Dictionary<ZoneOrigin, float>();
    Dictionary<ZoneOrigin, string> paramNames = new Dictionary<ZoneOrigin, string>();
    

    void Awake()
    {
        GlobalEventManager.EnterAudioZone.AddListener(OnEnterZone);
        GlobalEventManager.ExitAudioZone.AddListener(OnExitZone);
        //GlobalEventManager.OnCompleteQuest.AddListener(OnCompleteQuest); //hier fehler

        npcs.Add(ZoneOrigin.Racoon, AudioZone.Outside);
        npcs.Add(ZoneOrigin.Beaver, AudioZone.Outside);
        npcs.Add(ZoneOrigin.Jay, AudioZone.Outside);
        npcs.Add(ZoneOrigin.Boar, AudioZone.Outside);
        npcs.Add(ZoneOrigin.Wolf, AudioZone.Outside);
        oldValues.Add(ZoneOrigin.River, 0);
        oldValues.Add(ZoneOrigin.Racoon, 0);
        oldValues.Add(ZoneOrigin.Beaver, 0);
        oldValues.Add(ZoneOrigin.Jay, 0);
        oldValues.Add(ZoneOrigin.Boar, 0);
        oldValues.Add(ZoneOrigin.Wolf, 0);
        newValues.Add(ZoneOrigin.River, 0);
        newValues.Add(ZoneOrigin.Racoon, 0);
        newValues.Add(ZoneOrigin.Beaver, 0);
        newValues.Add(ZoneOrigin.Jay, 0);
        newValues.Add(ZoneOrigin.Boar, 0);
        newValues.Add(ZoneOrigin.Wolf, 0);
        paramNames.Add(ZoneOrigin.River, "River");
        paramNames.Add(ZoneOrigin.Racoon, "Racoon");
        paramNames.Add(ZoneOrigin.Beaver, "Beaver");
        paramNames.Add(ZoneOrigin.Jay, "Jay");
        paramNames.Add(ZoneOrigin.Boar, "Boar");
        //paramNames.Add(ZoneOrigin.Wolf, "Wolves");
    }

    void UpdateValues()
    {
        racoon = npcs[ZoneOrigin.Racoon];
        beaver = npcs[ZoneOrigin.Beaver];
        jay = npcs[ZoneOrigin.Jay];
        boar = npcs[ZoneOrigin.Boar];
        //wolf = npcs[ZoneOrigin.Wolf];
    }

    private IEnumerator WaitForLerpRiver(ZoneOrigin origin)
    {
        float time = 0;
        
        while (time < 1)
        {
            FmodEvents.instance._musicInstance.setParameterByName(paramNames[origin], (Mathf.Lerp(oldValues[origin], newValues[origin], time / 1)));
            //oldValue = Mathf.Lerp(oldValue, newValue, time / 1);
            time += Time.deltaTime;
            yield return null;
        }

        oldValues[origin] = newValues[origin];
    }

    private IEnumerator WaitForLerpNPC(ZoneOrigin origin)
    {
        float time = 0;
        
        while (time < 1)
        {
            FmodEvents.instance._musicInstance.setParameterByName(paramNames[origin], (Mathf.Lerp(oldValues[origin], newValues[origin], time / 1)));
            //oldValue = Mathf.Lerp(oldValue, newValue, time / 1);
            time += Time.deltaTime;
            yield return null;
        }
        oldValues[origin] = newValues[origin];
    }

    void CheckRiverZone()
    {
        //ToDo: Create New Better Zones
        switch (river)
        {
            case AudioZone.Outside:
                newValues[ZoneOrigin.River] = 0f;
                break;
            case AudioZone.Furthest:
                newValues[ZoneOrigin.River] = 0.2f;
                break;
            case AudioZone.Far:
                newValues[ZoneOrigin.River] = 0.4f;
                break;
            case AudioZone.Mid:
                newValues[ZoneOrigin.River] = 0.6f;
                break;
            case AudioZone.Close:
                newValues[ZoneOrigin.River] = 0.8f;
                break;
            case AudioZone.Closest:
                newValues[ZoneOrigin.River] = 1;
                break;
        }
        StartCoroutine(WaitForLerpRiver(ZoneOrigin.River));
    }

    void CheckNPCZone(ZoneOrigin origin)
    {
        switch (npcs[origin])
        {
            case AudioZone.Outside:
                newValues[origin] = 0f;
                break;
            case AudioZone.Furthest:
                newValues[origin] = 1f;
                break;
            case AudioZone.Far:
                newValues[origin] = 1.5f;
                break;
            case AudioZone.Mid:
                newValues[origin] = 2f;
                break;
            case AudioZone.Close:
                newValues[origin] = 2.5f;
                break;
            case AudioZone.Closest:
                newValues[origin] = 3f;
                break;
        }
        StartCoroutine(WaitForLerpNPC(origin));
    }
    void OnEnterZone(AudioZone zone, ZoneOrigin origin)
    {
        if (origin == ZoneOrigin.River)
        {
            river = zone;
            CheckRiverZone();
        }
        else if (origin == ZoneOrigin.Wolf)
        {
            wolf = zone;
            FmodEvents.instance._wolfMusicInstance.setParameterByName("Wolves", 1);
        }
        else
        {
            npcs[origin] = zone;
            CheckNPCZone(origin);
        }
        UpdateValues();

    }

    void OnExitZone(AudioZone zone, ZoneOrigin origin)
    {
        if (origin == ZoneOrigin.River)
        {
            river = (zone - 1);
            CheckRiverZone();
        }
        else if (origin == ZoneOrigin.Wolf)
        {
            wolf = AudioZone.Outside;
            FmodEvents.instance._wolfMusicInstance.setParameterByName("Wolves", 0);
        }
        else
        {
            npcs[origin] = (zone - 1);
            CheckNPCZone(origin);
        }
        UpdateValues();
    }

    void OnCompleteQuest(int questCount)
    {
        foreach (KeyValuePair<ZoneOrigin, AudioZone> zone in npcs)
        {
            npcs[zone.Key] = AudioZone.Outside;
        }
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
    Jay,
    Boar
}
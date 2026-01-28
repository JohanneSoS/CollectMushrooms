using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioZoneManager : MonoBehaviour
{

    public string currentCol;
    public bool atFarRiver = false;
    public bool atMidRiver = false;
    public bool atCloseRiver = false;
    public bool atRiverCoast = false;
    public bool insideRiver = false;
    public bool wolfZoneClose = false;
    public bool wolfZoneFar = false;
    public bool racoonFar = false;
    public bool racoonFurthest = false;
    public bool racoonClose = false;

    private float oldRiverValue = 0f;
    private float newRiverValue = 0f;

    private float oldRacoonValue = 0f;
    private float newRacoonValue = 0f;
    

    void Awake()
    {
        EventManager.EnterZone.AddListener(OnEnterZone);
        EventManager.ExitZone.AddListener(OnExitZone);
        EventManager.OnCompleteQuest.AddListener(OnCompleteQuest);
    }

    void Update()
    {

        CheckRiverZone();
        CheckWolfZone();
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
        if (!atFarRiver && !atMidRiver && !atCloseRiver && !atRiverCoast && !insideRiver)
        {
            newRiverValue = 0f;
        }
        else if (atFarRiver)
        {
            newRiverValue = 0.2f;
        }
        else if (atMidRiver & !atFarRiver)
        {
            newRiverValue = 0.4f;
        }
        else if (atCloseRiver & !atMidRiver)
        {
            newRiverValue = 0.6f;
        }
        else if (atRiverCoast & !atCloseRiver)
        {
            newRiverValue = 0.8f;
        }
        else if (insideRiver & !atRiverCoast)
        {
            newRiverValue = 1f;
        }
    }

    void CheckWolfZone()
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
    }

    void CheckNPCZone()
    {
        if (!racoonClose && !racoonFar && !racoonFurthest)
        {
            newRacoonValue = 0f;
            //FmodEvents.instance._musicInstance.setParameterByName("Racoon", 0f);
        }
        else if (racoonClose)
        {
            newRacoonValue = 3f;
            //FmodEvents.instance._musicInstance.setParameterByName("Racoon", 3f);
        }
        else if (racoonFar)
        {
            newRacoonValue = 2f;
            //FmodEvents.instance._musicInstance.setParameterByName("Racoon", 2f);
        }
        else if (racoonFurthest)
        {
            newRacoonValue = 1f;
            //FmodEvents.instance._musicInstance.setParameterByName("Racoon", 1f);
        }
    }
    void OnEnterZone(string origin)
    {
        Debug.Log(origin+ "Colider Entered");
        switch (origin)
        {
            case "FarRiver":
                atFarRiver = true;
                break;
            case "MidRiver":
                atMidRiver = true;
                break;
            case "CloseRiver":
                atCloseRiver = true;
                atRiverCoast = false;
                break;
            case "Water":
                insideRiver = true;
                atRiverCoast = false;
                break;
            case "WolfZoneClose":
                wolfZoneClose = true;
                break;
            case "WolfZoneFar":
                wolfZoneFar = true;
                break;
            case "RacoonZoneFurthest":
                racoonFurthest = true;
                break;
            case "RacoonZoneFar":
                racoonFar = true;
                racoonFurthest = false;
                break;
            case "RacoonZoneClose":
                racoonClose = true;
                racoonFurthest = false;
                racoonFar = false;
                break;
        }
    }

    void OnExitZone(string origin)
    {
        Debug.Log(origin + "Colider Exited");
        switch (origin)
        {
            case "FarRiver":
                atFarRiver = false;
                break;
            case "MidRiver":
                atMidRiver = false;
                break;
            case "CloseRiver":
                atCloseRiver = false;
                if (!atMidRiver)
                {
                    atRiverCoast = true;
                }
                break;
            case "Water":
                insideRiver = false;
                break;
            case "WolfZoneClose":
                wolfZoneClose = false;
                break;
            case "WolfZoneFar":
                wolfZoneFar = false;
                break;
            case "RacoonZoneFurthest":
                racoonFurthest = false;
                break;
            case "RacoonZoneFar":
                racoonFar = false;
                racoonFurthest = true;
                break;
            case "RacoonZoneClose":
                racoonClose = false;
                racoonFar = true;
                break;
        }
    }

    void OnCompleteQuest(int questCount)
    {
        racoonClose = false;
        racoonFar = false;
        racoonFurthest = false;
    }
}

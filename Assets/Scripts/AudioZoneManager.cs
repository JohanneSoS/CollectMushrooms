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

    private float oldRiverValue = 0f;
    private float newRiverValue = 0f;
    

    void Awake()
    {
        EventManager.EnterZone.AddListener(OnEnterZone);
        EventManager.ExitZone.AddListener(OnExitZone);
    }

    void Update()
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

        if (oldRiverValue != newRiverValue)
        {
            FmodEvents.instance._ambienceInstance.setParameterByName("River", (Mathf.Lerp(oldRiverValue, newRiverValue, 1)));
            StartCoroutine(WaitForLerp());
        }
    }

    private IEnumerator WaitForLerp()
    {
        yield return new WaitForSeconds(1);
        oldRiverValue = newRiverValue;
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
        }
    }
}

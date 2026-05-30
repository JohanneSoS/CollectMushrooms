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
    //private AudioZone wolf = AudioZone.Outside;

    public int currentNpcID = 0;

    Dictionary<ZoneOrigin, AudioZone> npcs = new Dictionary<ZoneOrigin, AudioZone>();
    Dictionary<ZoneOrigin, float> oldValues = new Dictionary<ZoneOrigin, float>();
    Dictionary<ZoneOrigin, float> newValues = new Dictionary<ZoneOrigin, float>();
    Dictionary<ZoneOrigin, string> paramNames = new Dictionary<ZoneOrigin, string>();
    

    void Awake()
    {
        GlobalEventManager.EnterAudioZone.AddListener(OnEnterZone);
        GlobalEventManager.ExitAudioZone.AddListener(OnExitZone);
        GlobalEventManager.OnStartQuest.AddListener(StartQuest);

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
    }

    void UpdateValues()
    {
        racoon = npcs[ZoneOrigin.Racoon];
        beaver = npcs[ZoneOrigin.Beaver];
        jay = npcs[ZoneOrigin.Jay];
        boar = npcs[ZoneOrigin.Boar];
    }
    void CheckRiverZone()
    {
        switch (river)
        {
            case AudioZone.Outside:
                newValues[ZoneOrigin.River] = 0f;
                break;
            case AudioZone.Far:
                newValues[ZoneOrigin.River] = 0.33f;
                break;
            case AudioZone.Near:
                newValues[ZoneOrigin.River] = 0.66f;
                break;
            case AudioZone.Close:
                newValues[ZoneOrigin.River] = 1f;
                break;
        }
        FmodEvents.instance._ambienceInstance.setParameterByName(paramNames[ZoneOrigin.River], newValues[ZoneOrigin.River]);
    }

    void CheckNPCZone(ZoneOrigin origin)
    {
        switch (npcs[origin])
        {
            case AudioZone.Outside:
                newValues[origin] = 0f;
                break;
            case AudioZone.Far:
                newValues[origin] = 0f;
                RuntimeManager.StudioSystem.setParameterByName("State", 0);
                break;
            case AudioZone.Near:
                newValues[origin] = 1f;
                RuntimeManager.StudioSystem.setParameterByName("State", 2);
                break;
            case AudioZone.Close:
                newValues[origin] = 2f;
                break;
        }
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("RangeToNPC", newValues[origin]);
    }
    void OnEnterZone(AudioZone zone, ZoneOrigin origin)
    {
        if (origin == ZoneOrigin.River)
        {
            river = zone;
            CheckRiverZone();
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
        else
        {
            npcs[origin] = (zone - 1);
            CheckNPCZone(origin);
        }
        UpdateValues();
    }

    void StartQuest(int questID)
    {
        switch (QuestManager.instance.quests[questID].npcType)
        {
            case NPC.Racoon:
                currentNpcID = 1;
                break;
            case NPC.Beaver:
                currentNpcID = 2;
                break;
            case NPC.Jay:
                currentNpcID = 3;
                break;
            case NPC.Boar:
                currentNpcID = 4;
                break;
        }
        FmodEvents.instance._npcMusicInstance.setParameterByName("NPC", currentNpcID);
    }
}

public enum AudioZone 
{
    Outside,
    Far,
    Near,
    Close
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
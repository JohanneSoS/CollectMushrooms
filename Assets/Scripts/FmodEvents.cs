using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class FmodEvents : MonoBehaviour
{

    [Header("Music Events")] 
    [SerializeField] private EventReference npcMusic;
    //[SerializeField] private EventReference wolfMusic;
    [SerializeField] private EventReference baseMusic;
    
    [Header("SFX Events")]
    [Header("UI")]
    [SerializeField] public EventReference buttonClick;
    [Header("Ambience")]
    [SerializeField] private EventReference ambience;
    [Header("PlayerSounds")]
    [SerializeField] private EventReference sniff;
    [SerializeField] private EventReference pickUpMushroom;
    [SerializeField] private EventReference deliverMushroom;
    [SerializeField] private EventReference eatMushroom;
    [SerializeField] public EventReference openChestUI;
    [SerializeField] private EventReference findRacoon;
    [SerializeField] private EventReference findBeaver;
    [SerializeField] private EventReference findJay;
    [SerializeField] private EventReference findBoar;
    [SerializeField] private EventReference walking;
    [SerializeField] private EventReference sleep;
    [SerializeField] public EventReference firstQuestConvo;
    [SerializeField] public EventReference startQuest;
    [SerializeField] public EventReference finishDelivery;
    [SerializeField] public EventReference finishQuest;

    public EventInstance _ambienceInstance;
    public EventInstance _npcMusicInstance;
    public EventInstance _wolfMusicInstance;
    public EventInstance _baseMusicInstance;
    public EventInstance _walkingInstance;

    public static FmodEvents instance;

    [SerializeField] private GameObject player;
    [SerializeField] private ClockManager clockManager;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerMovement playerMovement;
    private bool FacingState = false;
    private bool LerpState = false;
    private float currentState;
    private bool isEmergency = false;
    private float currentNonEmergencyState = 0f;

    public Vector2 currentNPCPos = Vector2.zero;

    public Direction targetDir;

    public FMOD.Studio.Bus Master;
    public FMOD.Studio.Bus Music;
    public FMOD.Studio.Bus SFX;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    

    private float currentHour;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
    }

    private void OnEnable()
    {
        GlobalEventManager.OnSniffing.AddListener(Sniff);
        GlobalEventManager.OnPickItem.AddListener(PickUpMushroom);
        GlobalEventManager.OnGiveItem.AddListener(DeliverMushroom);
        GlobalEventManager.OnEatItem.AddListener(EatItem);
        GlobalEventManager.UpdateHealthBar.AddListener(UpdateHealth);
        GlobalEventManager.UpdateExhaustionBar.AddListener(UpdateExhaustion);
        GlobalEventManager.UpdateHungerBar.AddListener(UpdateHunger);
        GlobalEventManager.OnMovement.AddListener(CheckIfFacingNPC);
        GlobalEventManager.GamePaused.AddListener(OnGamePaused);
        GlobalEventManager.OnSkipToDay.AddListener(Sleep);
        GlobalEventManager.OnWalkingStart.AddListener(EnableWalking);
        GlobalEventManager.OnWalkingStop.AddListener(DisableWalking);
        GlobalEventManager.ToggleUI.AddListener(OnUIToggle);
        GlobalEventManager.ResumeGame.AddListener(ButtonClick);
        GlobalEventManager.OnInteractWithNPC.AddListener(ButtonClick);
        GlobalEventManager.OpenSleepUI.AddListener(ButtonClick);

    }

    private void OnDisable()
    {
        GlobalEventManager.OnSniffing.RemoveListener(Sniff);
        GlobalEventManager.OnPickItem.RemoveListener(PickUpMushroom);
        GlobalEventManager.OnEatItem.RemoveListener(EatItem);
        GlobalEventManager.OnGiveItem.RemoveListener(DeliverMushroom);
        GlobalEventManager.ApplyDamage.RemoveListener(UpdateHealth);
    }

    void Start()
    {
        currentHour = clockManager.hours;
        _npcMusicInstance = RuntimeManager.CreateInstance(npcMusic);
        _npcMusicInstance.start();
        _ambienceInstance = RuntimeManager.CreateInstance(ambience);
        _ambienceInstance.start();
        //_wolfMusicInstance = RuntimeManager.CreateInstance(wolfMusic);
        _wolfMusicInstance.start();
        _baseMusicInstance = RuntimeManager.CreateInstance(baseMusic);
        _baseMusicInstance.start();
        _walkingInstance = RuntimeManager.CreateInstance(walking);
        _walkingInstance.start();
        _walkingInstance.setPaused(true);
        RuntimeManager.StudioSystem.setParameterByName("Base", 1);
        currentState = 0;
        RuntimeManager.StudioSystem.setParameterByName("State", currentState);
        _baseMusicInstance.setParameterByName("Sections", 0);
    }

    void Update()
    {
        Master.setVolume(masterVolume);
        Music.setVolume(musicVolume);
        SFX.setVolume(sfxVolume);
        if (currentHour != clockManager.hours)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("DayTime", clockManager.hours);
            currentHour = clockManager.hours;
        }
    }

    public void LevelMix(float newMasterVol)
    {
        masterVolume = newMasterVol;
    }

    public void LevelMusic(float newMusicVol)
    {
        musicVolume = newMusicVol;
    }

    public void LevelSFX(float newSFXVol)
    {
        sfxVolume = newSFXVol;
    }

    void CheckIfFacingNPC()
    {
        //move this function to be triggered by movement
        Vector2 playerPos = playerMovement.gameObject.transform.position;
        
        int dx = currentNPCPos.x.CompareTo(playerPos.x);
        int dy = currentNPCPos.y.CompareTo(playerPos.y);

        switch (dx, dy)
        {
            case (0, 1):
                targetDir = Direction.N;
                break;
            case (1, 1):
                targetDir = Direction.NE;
                break;
            case (1, 0):
                targetDir = Direction.E;
                break;
            case (1, -1):
                targetDir = Direction.SE;
                break;
            case (0, -1):
                targetDir = Direction.S;
                break;
            case (-1, -1):
                targetDir = Direction.SW;
                break;
            case (-1, 0):
                targetDir = Direction.W;
                break;
            case (-1, 1):
                targetDir = Direction.NW;
                break;
        }

        switch (targetDir)
        {
            case Direction.N:
                if ((playerMovement.charDir == Direction.NW || playerMovement.charDir == Direction.NE ||
                                     playerMovement.charDir == Direction.N))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
            case Direction.NE:
                if ((playerMovement.charDir == Direction.N || playerMovement.charDir == Direction.NE ||
                                     playerMovement.charDir == Direction.E))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
            case Direction.E:
                if ((playerMovement.charDir == Direction.NE || playerMovement.charDir == Direction.SE ||
                                     playerMovement.charDir == Direction.E))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
            case Direction.SE:
                if ((playerMovement.charDir == Direction.S || playerMovement.charDir == Direction.SE ||
                                     playerMovement.charDir == Direction.E))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
            case Direction.S:
                if ((playerMovement.charDir == Direction.S || playerMovement.charDir == Direction.SE ||
                                     playerMovement.charDir == Direction.SW))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
            case Direction.SW:
                if ((playerMovement.charDir == Direction.S || playerMovement.charDir == Direction.W ||
                                     playerMovement.charDir == Direction.SW))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
            case Direction.W:
                if ((playerMovement.charDir == Direction.W || playerMovement.charDir == Direction.SW ||
                                     playerMovement.charDir == Direction.NW))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
            case Direction.NW:
                if ((playerMovement.charDir == Direction.N || playerMovement.charDir == Direction.NW ||
                    playerMovement.charDir == Direction.W))
                {
                    
                    FacingState = true;
                }
                else
                {
                    
                    FacingState = false;
                }
                break;
        }
        if (FacingState == true && LerpState == false) //put facing logic in audiozones to be able to enable the music when in close zone
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FaceTowardsNPC", 1);
            LerpState = true;
        }
        else if (FacingState == false && LerpState == true)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FaceTowardsNPC", 0);
            LerpState = false;
        }
    }
    
    void UpdateHealth(int amount)
    {
        float currentHealth = playerStats.currentHealth;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Health", currentHealth);
        if (amount < 25)
        {
            isEmergency = true;
        }
        else if (playerStats.currentHealth >= 25 && playerStats.currentExhaustion >= 15 &&
                 playerStats.currentHunger >= 15)
        {
            isEmergency = false;
        }
        SwitchMusicState(currentNonEmergencyState);
    }

    void UpdateExhaustion(int amount)
    {
        float currentExhaustion = playerStats.currentExhaustion;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Exhaustion", currentExhaustion);
        if (amount < 15)
        {
            isEmergency = true;
        }
        else if (playerStats.currentHealth >= 25 && playerStats.currentExhaustion >= 15 &&
                 playerStats.currentHunger >= 15)
        {
            isEmergency = false;
        }
        SwitchMusicState(currentNonEmergencyState);
    }

    void UpdateHunger(int amount)
    {
        float currentHunger = playerStats.currentHunger;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Hunger", currentHunger);
        if (amount < 15)
        {
            isEmergency = true;
        }
        else if (playerStats.currentHealth >= 25 && playerStats.currentExhaustion >= 15 &&
                 playerStats.currentHunger >= 15)
        {
            isEmergency = false;
        }
        SwitchMusicState(currentNonEmergencyState);
    }

    void Sniff()
    {
        RuntimeManager.PlayOneShot(sniff, player.transform.position);
        _npcMusicInstance.getParameterByName("NPC", out var npcVal);
        if (LerpState)
        {
            switch (npcVal)
            {
                case 1:
                    StartCoroutine(PlayFindingNPCSound(findRacoon));
                    break;
                case 2:                    
                    StartCoroutine(PlayFindingNPCSound(findBeaver));
                    break;
                case 3:
                    StartCoroutine(PlayFindingNPCSound(findJay));
                    break;
                case 4:
                    StartCoroutine(PlayFindingNPCSound(findBoar));
                    break;
            }
        }
    }

    IEnumerator PlayFindingNPCSound(EventReference findType)
    {
        yield return new WaitForSeconds(0.3f);
        RuntimeManager.PlayOneShot(findType);
    }

    void PickUpMushroom()
    {
        RuntimeManager.PlayOneShot(pickUpMushroom, player.transform.position);
    }

    void DeliverMushroom()
    {
        RuntimeManager.PlayOneShot(deliverMushroom, player.transform.position);
    }

    void EatItem()
    {
        RuntimeManager.PlayOneShot(eatMushroom, player.transform.position);
    }

    void SkipDialogue()
    {
        RuntimeManager.PlayOneShot(buttonClick);
    }

    void Sleep()
    {
        RuntimeManager.PlayOneShot(sleep);
    }

    void ButtonClick()
    {
        RuntimeManager.PlayOneShot(buttonClick);
    }

    public void PlayOneShot(EventReference eventToPlay)
    {
        RuntimeManager.PlayOneShot(eventToPlay);  
    }
    
    void PlayOneShotAtPos(EventReference eventToPlay, Vector3 pos)
    {
        RuntimeManager.PlayOneShot(eventToPlay, pos);    
    }

    void PlayOneShotAtPlayerPos(EventReference eventToPlay)
    {
        RuntimeManager.PlayOneShot(eventToPlay, player.transform.position);   
    }

    void EnableWalking()
    {
        _walkingInstance.setPaused(false);
    }

    void DisableWalking()
    {
        _walkingInstance.setPaused(true);
    }

    void OnUIToggle(bool state)
    {
        if (state)
        {
            _walkingInstance.setPaused(true);
        }
    }

    void OnGamePaused(bool uiState)
    {
        /*if (uiState)
        {
            RuntimeManager.StudioSystem.getParameterByName("State", out float currentStateF);
            currentState = currentStateF;
            RuntimeManager.StudioSystem.setParameterByName("State", 1);
        }
        else
        {
            RuntimeManager.StudioSystem.setParameterByName("State", currentState);
        }*/
    }

    public void SwitchMusicState(float newState)
    {
        currentNonEmergencyState = newState;
        if (!isEmergency)
        {
            RuntimeManager.StudioSystem.setParameterByName("State", newState);
            currentState = newState;
        }
        else
        {
            RuntimeManager.StudioSystem.setParameterByName("State", 0);
            currentState = 0;
        }
    }

}

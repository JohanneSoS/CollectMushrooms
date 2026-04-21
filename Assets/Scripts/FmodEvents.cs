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
    [SerializeField] private EventReference music;
    [SerializeField] private EventReference wolfMusic;
    [SerializeField] private EventReference baseMusic;
    
    [Header("SFX Events")]
    [Header("UI")]
    [SerializeField] private EventReference buttonClick;
    [Header("Ambience")]
    [SerializeField] private EventReference ambience;
    [Header("PlayerSounds")]
    [SerializeField] private EventReference sniff;
    [SerializeField] private EventReference pickUpMushroom;
    [SerializeField] private EventReference deliverMushroom;
    [SerializeField] private EventReference eatMushroom;
    [SerializeField] private EventReference openChestUI;

    public EventInstance _ambienceInstance;
    public EventInstance _musicInstance;
    public EventInstance _wolfMusicInstance;
    public EventInstance _baseMusicInstance;

    public static FmodEvents instance;

    [SerializeField] private GameObject player;
    [SerializeField] private ClockManager clockManager;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerMovement playerMovement;
    private bool FacingState = false;
    private bool LerpState = false;

    public Vector2 currentNPCPos = Vector2.zero;

    public Direction targetDir;
    //[SerializeField] private PlayerMovement playerMovement;
  

    private float currentHour;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void OnEnable()
    {
        GlobalEventManager.OnSniffing.AddListener(Sniff);
        GlobalEventManager.OnPickItem.AddListener(PickUpMushroom);
        GlobalEventManager.OnGiveItem.AddListener(DeliverMushroom);
        GlobalEventManager.OnEatItem.AddListener(EatItem);
        GlobalEventManager.ApplyDamage.AddListener(UpdateHealth);
        GlobalEventManager.ApplyHeal.AddListener(UpdateHealth);
        GlobalEventManager.UpdateExhaustionBar.AddListener(UpdateExhaustion);
        GlobalEventManager.UpdateHungerBar.AddListener(UpdateHunger);
        GlobalEventManager.OnMovement.AddListener(CheckIfFacingNPC);
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
        _musicInstance = RuntimeManager.CreateInstance(music);
        //_musicInstance.start();
        _ambienceInstance = RuntimeManager.CreateInstance(ambience);
        _ambienceInstance.start();
        _wolfMusicInstance = RuntimeManager.CreateInstance(wolfMusic);
        _wolfMusicInstance.start();
        _baseMusicInstance = RuntimeManager.CreateInstance(baseMusic);
        _baseMusicInstance.start();
        RuntimeManager.StudioSystem.setParameterByName("Base", 1); 
        //Check Name
        //_musicInstance.setParameterByName("", 1);
    }

    void Update()
    {
        if (currentHour != clockManager.hours)
        {
            //_ambienceInstance.setParameterByName("DayTime", clockManager.hours);
            //_musicInstance.setParameterByName("DayTime", clockManager.hours);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("DayTime", clockManager.hours);
            currentHour = clockManager.hours;
        }
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

        if (FacingState == true && LerpState == false)
        {
            StartCoroutine(FaceTowardsNPC());
            LerpState = true;
        }
        else if (FacingState == false && LerpState == true)
        {
            StartCoroutine(FaceAwayFromNPC());
            LerpState = false;
        }
    }
    
    private IEnumerator FaceTowardsNPC()
    {
        float time = 0;
        
        while (time < 1)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FaceTowardsNPC", (Mathf.Lerp(0, 1, time / 1)));
            time += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator FaceAwayFromNPC()
    {
        float time = 0;
        
        while (time < 1)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("FaceTowardsNPC", (Mathf.Lerp(1, 0, time / 1)));
            time += Time.deltaTime;
            yield return null;
        }
    }

    void UpdateHealth(int amount)
    {
        float currentHealth = playerStats.currentHealth;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Health", currentHealth);
    }

    void UpdateExhaustion(int amount)
    {
        float currentExhaustion = playerStats.currentExhaustion;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Exhaustion", currentExhaustion);
    }

    void UpdateHunger(int amount)
    {
        float currentHunger = playerStats.currentHunger;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Hunger", currentHunger);
    }

    void Sniff()
    {
        RuntimeManager.PlayOneShot(sniff, player.transform.position);
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

    
}

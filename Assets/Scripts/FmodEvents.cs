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

    public static FmodEvents instance;

    [SerializeField] private GameObject player;
    [SerializeField] private ClockManager clockManager;
    [SerializeField] private PlayerStats playerStats;
  

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
        GlobalEventManager.ApplyDamage.AddListener(UpdateHealth);
        GlobalEventManager.UpdateExhaustionBar.AddListener(UpdateExhaustion);
        GlobalEventManager.UpdateHungerBar.AddListener(UpdateHunger);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnSniffing.RemoveListener(Sniff);
        GlobalEventManager.OnPickItem.RemoveListener(PickUpMushroom);
        GlobalEventManager.OnGiveItem.RemoveListener(DeliverMushroom);
        GlobalEventManager.ApplyDamage.RemoveListener(UpdateHealth);
    }

    void Start()
    {
        currentHour = clockManager.hours;
        _musicInstance = RuntimeManager.CreateInstance(music);
        _musicInstance.start();
        _ambienceInstance = RuntimeManager.CreateInstance(ambience);
        _ambienceInstance.start();
        //_musicInstance.setParameterByName("", 1);
    }

    void Update()
    {
        if (currentHour != clockManager.hours)
        {
            _ambienceInstance.setParameterByName("DayTime", clockManager.hours);
            _musicInstance.setParameterByName("DayTime", clockManager.hours);
            //_musicInstance.setParameterByName("Music", clockManager.hours);
            currentHour = clockManager.hours;
        }
    }

    void UpdateHealth(int amount)
    {
        float currentHealth = playerStats.currentHealth;
        _musicInstance.setParameterByName("Health", currentHealth);
    }

    void UpdateExhaustion(int amount)
    {
        float currentExhaustion = playerStats.currentExhaustion;
        _musicInstance.setParameterByName("Exhaustion", currentExhaustion);
    }

    void UpdateHunger(int amount)
    {
        float currentHunger = playerStats.currentHunger;
        _musicInstance.setParameterByName("Hunger", currentHunger);
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
}

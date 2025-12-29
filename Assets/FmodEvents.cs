using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;
using System.Collections;
using System.Collections.Generic;

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

    public static FmodEvents instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
    
    
}

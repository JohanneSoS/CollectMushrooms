using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MushroomLighter : MonoBehaviour
{
    private Light2D mushroomLight;
    [SerializeField] private float lightIntensity;
    
    void Start()
    {
        mushroomLight = GetComponent<Light2D>();
        mushroomLight.intensity = 0f;
    }

    void Awake()
    {
        GlobalEventManager.OnSniffing.AddListener(ActivateLight);
        GlobalEventManager.OnSniffingEnd.AddListener(DeactivateLight);
    }
    
    void ActivateLight()
    {
        mushroomLight.intensity = lightIntensity;
    }
    

    void DeactivateLight()
    {
        mushroomLight.intensity = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MushroomLighter : MonoBehaviour
{
    private Light2D mushroomLight;
    [SerializeField] private float lightIntensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
 
    void Start()
    {
        mushroomLight = GetComponent<Light2D>();
        mushroomLight.intensity = 0f;
    }

    void Awake()
    {
        EventManager.OnSniffing.AddListener(ActivateLight);
        EventManager.OnSniffingEnd.AddListener(DeactivateLight);
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

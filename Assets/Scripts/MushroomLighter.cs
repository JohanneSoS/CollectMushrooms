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
    }
    
    void ActivateLight(float sniffingDuration)
    {
        mushroomLight.intensity = lightIntensity;
        StartCoroutine(LightingCoroutine(sniffingDuration));
        
    }
    
    IEnumerator LightingCoroutine(float sniffingDuration)
    {
        yield return new WaitForSeconds(sniffingDuration);
        DeactivateLight();
    }

    public void DeactivateLight()
    {
        mushroomLight.intensity = 0f;
    }
}

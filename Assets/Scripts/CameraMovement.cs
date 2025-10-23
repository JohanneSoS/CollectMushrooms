using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using Unity.Mathematics;

public class CameraMovement : MonoBehaviour
{
    public float duration;
    [SerializeField] private CinemachineBasicMultiChannelPerlin noiseAttribute;
    [SerializeField] private float shakeIntensity;

    void Awake()
    {
        EventManager.OnSniffing.AddListener(StartShake);
    }
    
    public void StartShake()
    {
        StartCoroutine(Shaking());
    }
    IEnumerator Shaking()
    {
        //Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            noiseAttribute.AmplitudeGain = shakeIntensity;
            //transform.position = startPos + Random.insideUnitSphere;
            yield return null;
        }
        noiseAttribute.AmplitudeGain = 0f;
        //transform.position = startPos;
    }

}

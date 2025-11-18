using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using Unity.Mathematics;

public class CameraMovement : MonoBehaviour
{
    public float duration;
    [SerializeField] private CinemachineBasicMultiChannelPerlin noiseAttribute;
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float zoomDuration;
    public CinemachineCamera virtualCamera;
    private float camDistance;
    private float defaultCamDistance;
    private float newCamDistance;
    private bool zoomActive = false;
    

    void Awake()
    {
        defaultCamDistance = virtualCamera.Lens.OrthographicSize;
        camDistance = virtualCamera.Lens.OrthographicSize;
        EventManager.OnSniffing.AddListener(StartShake);
        EventManager.OnChangeZoomForArea.AddListener(ChangeZoomForArea);
        EventManager.OnResetZoom.AddListener(ResetZoom);
    }

    void Update()
    {
        var t = Time.deltaTime * zoomDuration;
        if (zoomActive)
        {
            if (virtualCamera.Lens.OrthographicSize != newCamDistance)
            {
                virtualCamera.Lens.OrthographicSize = Mathf.Lerp(camDistance, newCamDistance, t);
                camDistance = virtualCamera.Lens.OrthographicSize;
            }
            else
            {
                zoomActive = false;
                camDistance = newCamDistance;
            }
        }
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

    void ChangeZoomForArea(float zoom)
    {
        newCamDistance = zoom;
        zoomActive = true;
    }

    void ResetZoom()
    {
        newCamDistance = defaultCamDistance;
        zoomActive = true;
    }
    
}

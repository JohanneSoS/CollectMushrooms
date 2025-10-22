using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Cache = UnityEngine.Cache;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private CharacterRenderer charRenderer;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private HideLayer hideLayerTree;
    [SerializeField] private HideLayer hideLayerBushes;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private Light2D charLight;
    
    [Header("Parameters")]
    [SerializeField] private float movementSpeed;
    [SerializeField] public float sniffDuration;
    [SerializeField] private float sniffCooldown;
    [SerializeField] private float sniffLightIntensity;

    public bool sniffActive;
    private bool canSniff = true;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        charRenderer = GetComponentInChildren<CharacterRenderer>();
    }

    void Update()
    {
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.deltaTime;
        rbody.MovePosition(newPos);
        
        if (inputVector != Vector2.zero && !charRenderer.isRunning)
        {
            charRenderer.isRunning = true;
            charRenderer.CheckRunningState();
        }

        if (inputVector == Vector2.zero && charRenderer.isRunning)
        {
            charRenderer.isRunning = false;
            charRenderer.CheckRunningState();
        }

        if (inputVector.x < 0 && charRenderer.isFlipped)
        {
            charRenderer.FlipSprite("left");
        }
        else if (inputVector.x > 0 && !charRenderer.isFlipped)
        {
            charRenderer.FlipSprite("right");
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Sniff();
        }
    }

    private void Sniff()
    {
        if (sniffActive != true)
        {
            EventManager.OnSniffing.Invoke(sniffDuration);
            hideLayerTree.Hide();
            hideLayerBushes.Hide();
            StartCoroutine(SniffDuration());
        }
    }

    IEnumerator SniffDuration()
    {
        canSniff = false;
        sniffActive = true;
        cameraMovement.StartShake();
        float defaultLightIntensity = charLight.intensity;
        charLight.intensity = sniffLightIntensity;
        yield return new WaitForSeconds(sniffDuration);
        sniffActive = false;
        hideLayerTree.Show();
        hideLayerBushes.Show();
        charLight.intensity = defaultLightIntensity;
        yield return new WaitForSeconds(sniffCooldown);
    }
    
}

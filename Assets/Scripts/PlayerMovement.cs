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

        EventManager.OnDayStart.AddListener(DayStart);
        EventManager.OnNightStart.AddListener(NightStart);
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
            EventManager.OnWalkingStart.Invoke();
            charRenderer.CheckRunningState();
        }

        if (inputVector == Vector2.zero && charRenderer.isRunning)
        {
            charRenderer.isRunning = false;
            EventManager.OnWalkingStop.Invoke();
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
        if (sniffActive != true && canSniff)
        {
            EventManager.OnSniffing.Invoke();
            StartCoroutine(SniffDuration());
        }
    }

    IEnumerator SniffDuration()
    {
        canSniff = false;
        sniffActive = true;
        float defaultLightIntensity = charLight.intensity;
        charLight.intensity = sniffLightIntensity;
        yield return new WaitForSeconds(sniffDuration);
        sniffActive = false;
        EventManager.OnSniffingEnd.Invoke();
        charLight.intensity = defaultLightIntensity;
        yield return new WaitForSeconds(sniffCooldown);
        canSniff = true;
    }

    private void DayStart()
    {
        charLight.enabled = false;
        print("Player realises Day started");
    }

    private void NightStart()
    {
        charLight.enabled = true;
        print("Player realises Night started");
    }
    
}

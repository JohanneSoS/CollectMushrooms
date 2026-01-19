using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cache = UnityEngine.Cache;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    
    [SerializeField] private CharacterRenderer charRenderer;
    [SerializeField] private Rigidbody2D rbody;
    //[SerializeField] private Light2D charLight;
    [SerializeField] private Collider2D riverCol;
    
    [Header("Parameters")]
    [SerializeField] private float movementSpeed;
    
    //[SerializeField] public float sniffDuration;
    //[SerializeField] private float sniffCooldown;
    
    [SerializeField] private float swimmingSlowFactor;
    
    //[SerializeField] private float lightIntensityEvening;
    //[SerializeField] private float lightIntensityNight;

    //public bool sniffActive;
    //private bool canSniff = true;
    private bool riverHovering = false;
    private bool isSwimming = false;
    //private bool isNight = false;
    //private bool isEvening = false;

    public bool uiActive;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        charRenderer = GetComponentInChildren<CharacterRenderer>();
        EventManager.ToggleUI.AddListener(ToggleUI);
    }
    
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        List<Sprite> directionSprite = charRenderer.GetSpriteDirection(inputVector);
        charRenderer.UpdateSprite(directionSprite);
        Vector2 movement = inputVector * movementSpeed * swimmingSlowFactor;
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

        if (inputVector.x < 0)
        {
            charRenderer.FlipSprite("right");
        }
        else if (inputVector.x > 0)
        {
            charRenderer.FlipSprite("left");
        }
        /*if (!uiActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var selectedItem = InventoryManager.instance.GetSelectedItem(false);
                StartCoroutine(CheckIfCanEat(selectedItem));
            }
        }*/
    }
    
    /*IEnumerator CheckIfCanEat(Item selectedItem)
    {
        yield return new WaitForSeconds(0.2f);
        if (selectedItem.canEat && currentHunger < maxHunger && !isCollecting)
        {
            EventManager.HealHunger.Invoke(selectedItem.hungerAmount);
            Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
        }
    }*/
    
    private void EnableSwimming()
    {
        riverCol.isTrigger = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("River"))
        {
            riverHovering = true;
            isSwimming = true;
            swimmingSlowFactor = 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("River"))
        {
            riverHovering = false;
            isSwimming = false;
            swimmingSlowFactor = 1f;
        }
    }

    private void ToggleUI(bool uiState)
    {
        uiActive = uiState;
    }
}
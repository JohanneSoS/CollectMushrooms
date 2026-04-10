using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using Cache = UnityEngine.Cache;
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

    public Direction charDir;
    
    //[SerializeField] public float sniffDuration;
    //[SerializeField] private float sniffCooldown;
    
    [SerializeField] private float swimmingSlowFactor;
    [SerializeField] private Vector3 spawnPos;
    
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
        GlobalEventManager.ToggleUI.AddListener(ToggleUI);
        GlobalEventManager.OnRespawn.AddListener(Respawn);
        GlobalEventManager.ChangeMovementSpeed.AddListener(ChangeMovementSpeed);
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
        CheckDir(inputVector);
        Vector2 movement = inputVector * movementSpeed * swimmingSlowFactor;
        Vector2 newPos = currentPos + movement * Time.deltaTime;
        rbody.MovePosition(newPos);
        
        if (inputVector != Vector2.zero && !charRenderer.isRunning)
        {
            charRenderer.isRunning = true;
            GlobalEventManager.OnWalkingStart.Invoke();
            charRenderer.CheckRunningState();
        }

        if (inputVector == Vector2.zero && charRenderer.isRunning)
        {
            charRenderer.isRunning = false;
            GlobalEventManager.OnWalkingStop.Invoke();
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

    void Respawn()
    {
        transform.position = spawnPos;
    }

    void ChangeMovementSpeed(float movementSpeedMultiplier, float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(BuffMovementSpeed(movementSpeedMultiplier, duration));
        }
    }

    IEnumerator BuffMovementSpeed(float multiplier, float time)
    {
        float currentMovementSpeed = movementSpeed;
        movementSpeed = movementSpeed * multiplier;
        yield return new WaitForSeconds(time);
        movementSpeed = currentMovementSpeed;
    }

    void CheckDir(Vector2 input)
    {
        float deadzone = 0.1f;
        int xInput = Mathf.Abs(input.x) < deadzone ? 0 : (int)Mathf.Sign(input.x);
        int yInput = Mathf.Abs(input.y) < deadzone ? 0 : (int)Mathf.Sign(input.y);

        switch (yInput, xInput)
        {
            case (1,0):
                charDir = Direction.N;
                break;
            case (1, 1):
                charDir = Direction.NE;
                break;
            case (0, 1):
                charDir = Direction.E;
                break;
            case (-1,1):
                charDir = Direction.SE;
                break;
            case (-1, 0):
                charDir = Direction.S;
                break;
            case (-1, -1):
                charDir = Direction.SW;
                break;
            case (0, -1):
                charDir = Direction.W;
                break;
            case (1, -1):
                charDir = Direction.NW;
                break;
        }
    }
}

public enum Direction
{
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW
}
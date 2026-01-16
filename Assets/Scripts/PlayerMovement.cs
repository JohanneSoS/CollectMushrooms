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
    
    [SerializeField] private CharacterRenderer charRenderer;
    [SerializeField] private Rigidbody2D rbody;
    [SerializeField] private Light2D charLight;
    [SerializeField] private Collider2D riverCol;
    
    [Header("Parameters")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxHealth;
    [SerializeField] public int currentHealth;

    [SerializeField] private int maxHunger;
    [SerializeField] public int currentHunger;
    [SerializeField] private int maxExhaustion;
    [SerializeField] public int currentExhaustion;
    
    [SerializeField] public float sniffDuration;
    [SerializeField] private float sniffCooldown;
    [SerializeField] private float sniffLightRadius;
    
    [SerializeField] private float swimmingSlowFactor;
    
    [SerializeField] private float lightIntensityEvening;
    [SerializeField] private float lightIntensityNight;

    public bool sniffActive;
    private bool canSniff = true;
    private bool riverHovering = false;
    private bool isSwimming = false;
    private bool changeLightIntensity = false;
    private bool isNight = false;
    private bool isEvening = false;

    public bool uiActive;
    public bool isCollecting;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        charRenderer = GetComponentInChildren<CharacterRenderer>();

        EventManager.OnDayStart.AddListener(DayStart);
        EventManager.OnEveningStart.AddListener(EveningStart);
        EventManager.OnNightStart.AddListener(NightStart);
        EventManager.ApplyDamage.AddListener(RecieveDmg);
        EventManager.ApplyHeal.AddListener(HealHealth);
        EventManager.ApplyExhaustion.AddListener(ApplyExhaustion);
        EventManager.ResetExhaustion.AddListener(ResetExhaustion);
        EventManager.ApplyHunger.AddListener(ApplyHunger);
        EventManager.HealHunger.AddListener(HealHunger);
        EventManager.ToggleUI.AddListener(ToggleUI);
        EventManager.OnPickItem.AddListener(ToggleIsCollecting);
    }

    private void Start()
    {
        HealFully();
        ResetExhaustion();
        ResetHunger();
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

        if (!uiActive)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var selectedItem = InventoryManager.instance.GetSelectedItem(false);
                StartCoroutine(CheckIfCanEat(selectedItem));
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                Sniff();
                //EnableSwimming();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventManager.PauseGame.Invoke();
            }
        }

        if (currentHealth >= maxHealth) { currentHealth = maxHealth; }
        if (currentHunger >= maxHunger) { currentHunger = maxHunger; }
        if (currentExhaustion >= maxExhaustion) { currentExhaustion = maxExhaustion; }
        
        UpdateLightCircle();
    }
    
    IEnumerator CheckIfCanEat(Item selectedItem)
    {
        yield return new WaitForSeconds(0.2f);
        if (selectedItem.canEat && currentHunger < maxHunger && !isCollecting)
        {
            EventManager.HealHunger.Invoke(selectedItem.hungerAmount);
            Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
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
        float defaultLightRadius = charLight.pointLightOuterRadius;
        if (charLight.enabled == true)
        {
            charLight.pointLightOuterRadius = sniffLightRadius;
        }
        
        yield return new WaitForSeconds(sniffDuration);
        sniffActive = false;
        EventManager.OnSniffingEnd.Invoke();
        if (charLight.enabled == true)
        {
            charLight.pointLightOuterRadius = defaultLightRadius;
        }
        yield return new WaitForSeconds(sniffCooldown);
        canSniff = true;
    }

    private void DayStart()
    {
        charLight.enabled = false;
        charLight.intensity = 0f;
        print("Player realises Day started");
    }

    private void EveningStart()
    {
        changeLightIntensity = true;
        charLight.enabled = true;
        isNight = false;
    }

    private void NightStart()
    {
        changeLightIntensity = true;
        charLight.enabled = true;
        isNight = true;
        print("Player realises Night started");
    }

    private void UpdateLightCircle()
    {
        var t = Time.deltaTime * 1f;
        if (changeLightIntensity)
        {
            if (isNight)
            {
                if (charLight.intensity != lightIntensityNight)
                {
                    charLight.intensity = Mathf.Lerp(charLight.intensity, lightIntensityNight, t);
                }
                else
                {
                    changeLightIntensity = false;
                }
            }
            else if (!isNight)
            {
                if (charLight.intensity != lightIntensityEvening)
                {
                    charLight.intensity = Mathf.Lerp(charLight.intensity, lightIntensityEvening, t);
                }
                else
                {
                    changeLightIntensity = false;
                }
            }
        }
    }

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

    private void RecieveDmg(int damage)
    {
        currentHealth = currentHealth - damage;
        EventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void HealHealth(int healAmount)
    {
        currentHealth = currentHealth + healAmount;
        EventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void ApplyExhaustion(int exhaustionValue)
    {
        currentExhaustion = currentExhaustion - exhaustionValue;
        EventManager.UpdateExhaustionBar.Invoke(currentExhaustion);
    }

    private void ApplyHunger(int hungerValue)
    {
        currentHunger = currentHunger - hungerValue;
        EventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void HealHunger(int hungerValue)
    {
        currentHunger = currentHunger + hungerValue;
        EventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void HealFully()
    {
        currentHealth = maxHealth;
        EventManager.UpdateHealthBar.Invoke(currentHealth);
    }

    private void ResetHunger()
    {
        currentHunger = maxHunger;
        EventManager.UpdateHungerBar.Invoke(currentHunger);
    }

    private void ResetExhaustion()
    {
        currentExhaustion = maxExhaustion;
        EventManager.UpdateExhaustionBar.Invoke(currentExhaustion);
    }

    private void ToggleUI(bool uiState)
    {
        uiActive = uiState;
    }

    private void ToggleIsCollecting()
    {
        isCollecting = true;
        StartCoroutine(WaitForCollecting());
    }

    IEnumerator WaitForCollecting()
    {
        yield return new WaitForSeconds(0.3f);
        isCollecting = false;
    }
}
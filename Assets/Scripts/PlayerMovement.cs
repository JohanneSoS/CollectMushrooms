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
    [SerializeField] private Collider2D riverCol;
    
    [Header("Parameters")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    
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

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        charRenderer = GetComponentInChildren<CharacterRenderer>();

        EventManager.OnDayStart.AddListener(DayStart);
        EventManager.OnEveningStart.AddListener(EveningStart);
        EventManager.OnNightStart.AddListener(NightStart);
        EventManager.ApplyDamage.AddListener(RecieveDmg);
        EventManager.ApplyHeal.AddListener(HealHealth);
    }

    private void Start()
    {
        HealFully();
    }

    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
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
            EnableSwimming();
        }

        UpdateLightCircle();
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

    private void HealFully()
    {
        currentHealth = maxHealth;
        EventManager.UpdateHealthBar.Invoke(currentHealth);
    }
}

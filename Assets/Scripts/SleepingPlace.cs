using System;
using UnityEngine;

public class SleepingPlace : MonoBehaviour
{
    //[SerializeField] private Sprite[] sleepingPlaceSprites;
    [SerializeField] private SpriteRenderer frontRenderer;
    [SerializeField] private SpriteRenderer backRenderer;
    
    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite[] backSprites;

    [SerializeField] private int upgradeStage;
    private bool playerHovering = false;
    private bool isUpgradable = false;
    private bool canSleep = false;

    [SerializeField] private int sleepHealAmount;

    void Awake()
    {
        EventManager.OnDayStart.AddListener(DenySleeping);
        EventManager.OnEveningStart.AddListener(AllowSleeping);
    }
    
    void Start()
    {
        upgradeStage = 0;
        isUpgradable = true;
        ChangeSprites(upgradeStage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHovering)
        {
            Interact();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Hide();
            playerHovering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Show();
            playerHovering = false;
        }
    }

    private void Interact()
    {
        if (isUpgradable){ UpgradeBase(); }
        else if (canSleep) { Sleep(); }
    }
    private void UpgradeBase()
    {
        upgradeStage++;
        ChangeSprites(upgradeStage);
        EventManager.OnBaseUpgrade.Invoke();
        isUpgradable = false;
    }

    private void AllowSleeping()
    {
        canSleep = true;
    }

    private void DenySleeping()
    {
        canSleep = false;
    }
    
    private void Sleep()
    {
        EventManager.OnSkipToDay.Invoke();
        EventManager.ApplyHeal.Invoke(sleepHealAmount);
    }

    private void ChangeSprites(int id)
    {
        frontRenderer.sprite = frontSprites[id];
        backRenderer.sprite = backSprites[id];
    }
        
}

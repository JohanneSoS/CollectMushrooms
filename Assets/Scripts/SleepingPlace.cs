using System;
using UnityEngine;

public class SleepingPlace : MonoBehaviour
{
    [SerializeField] private SpriteRenderer frontRenderer;
    [SerializeField] private SpriteRenderer backRenderer;
    
    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite[] backSprites;

    [SerializeField] private int upgradeStage;
    private bool playerHovering = false;
    private bool isUpgradable = false;

    [SerializeField] private int sleepHealAmount;


    void Awake()
    {
        GlobalEventManager.BaseUpgrade.AddListener(UpgradeBase);
    }
    void Start()
    {
        upgradeStage = 0;
        isUpgradable = true;
        ChangeSprites(upgradeStage);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = false;
        }
    }
    private void UpgradeBase()
    {
        if (isUpgradable && playerHovering)
        {
            upgradeStage++;
            ChangeSprites(upgradeStage);
            isUpgradable = false;
        }
    }

    private void ChangeSprites(int id)
    {
        frontRenderer.sprite = frontSprites[id];
        backRenderer.sprite = backSprites[id];
    }
        
}

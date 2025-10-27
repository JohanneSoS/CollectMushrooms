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

    void Start()
    {
        upgradeStage = 0;
        isUpgradable = true;
        ChangeSprites(upgradeStage);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHovering && isUpgradable)
        {
            UpgradeBase();
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

    private void UpgradeBase()
    {
        upgradeStage++;
        ChangeSprites(upgradeStage);
        EventManager.OnBaseUpgrade.Invoke();
        isUpgradable = false;
    }

    private void ChangeSprites(int id)
    {
        frontRenderer.sprite = frontSprites[id];
        backRenderer.sprite = backSprites[id];
    }
        
}

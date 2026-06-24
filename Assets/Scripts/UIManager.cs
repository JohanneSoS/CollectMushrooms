using System.Collections;
using NUnit.Framework.Constraints;
using TMPro;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject SleepMenu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private GameObject [] QuestMenu;
    [SerializeField] private QuestRecipient [] Boxes;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private TextMeshProUGUI MenuButtonText;

    public bool uiActive = false;
    public string currentMenu = "none";

    public int activeBox = 0;

    void Awake()
    {
        GlobalEventManager.OpenSleepUI.AddListener(ActivateSleepMenu);
        GlobalEventManager.ToggleUI.AddListener(OnUIToggle);
        GlobalEventManager.OpenQuestUI.AddListener(OpenQuestUI);
        GlobalEventManager.CloseQuestUI.AddListener(ResumeGame);
        GlobalEventManager.PauseGame.AddListener(ActivatePauseMenu);
        GlobalEventManager.ResumeGame.AddListener(ResumeGame);
        GlobalEventManager.ConfirmUI.AddListener(ConfirmUI);
        GlobalEventManager.OnGameOver.AddListener(OnGameOver);
        LoadRequiredBoxSlots();
    }

    void Start()
    {
        GlobalEventManager.PauseGame.Invoke();
        MenuButtonText.text = "Start Game!";
    }
    
    private void OnUIToggle(bool uiState)
    {
        uiActive = uiState;
    }

    public void ConfirmUI()
    {
        FmodEvents.instance.PlayOneShot(FmodEvents.instance.buttonClick);
        switch (currentMenu)
        {
            case "pause":
                ResumeGame();
                break;
            case "gameover":
                GlobalEventManager.OnRespawn.Invoke();
                ResumeGame();
                break;
            case "sleep":
                SkipToDay();
                break;
        }
    }

    void ActivateSleepMenu()
    {
        SleepMenu.SetActive(true);
        currentMenu = "sleep";
        uiActive = true;
        GlobalEventManager.ToggleUI.Invoke(true);
        GlobalEventManager.GamePaused.Invoke(true);
    }

    void ActivatePauseMenu()
    {
        MenuButtonText.text = "Resume Game";
        PauseMenu.SetActive(true);
        currentMenu = "pause";
        uiActive = true;
        GlobalEventManager.ToggleUI.Invoke(true);
        GlobalEventManager.GamePaused.Invoke(true);
    }

    void ActivateGameOverMenu()
    {
        GameOverMenu.SetActive(true);
        currentMenu = "gameover";
        uiActive = true;
        GlobalEventManager.ToggleUI.Invoke(true);
    }

    public void ResumeGame()
    {
        SleepMenu.SetActive(false);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        for (int i = 0; i < QuestMenu.Length; i++)
        {
            QuestMenu[i].SetActive(false);
        }
        uiActive = false;
        currentMenu = "none";
        FmodEvents.instance.isMusicSilenced = false;
        FmodEvents.instance.SwitchMusicState();
        GlobalEventManager.ToggleUI.Invoke(false);
        GlobalEventManager.GamePaused.Invoke(false);
    }

    public void SkipToDay()
    {
        GlobalEventManager.OnSkipToDay.Invoke();
        ResumeGame();
    }
    
    void OpenQuestUI()
    {
        QuestMenu[(activeBox)].SetActive(true);
        currentMenu = "quest";
        uiActive = true;
        GlobalEventManager.ToggleUI.Invoke(true);
        FmodEvents.instance.PlayOneShot(FmodEvents.instance.openChestUI);
    }
    void OnGameOver(string deathReason)
    {
        switch (deathReason)
        {
            case "hunger":
                GameOverText.text = "You starved!";
                break;
            case "exhaustion":
                GameOverText.text = "You died of Exhaustion!";
                break;
            case "health":
                GameOverText.text = "You have been slain by a wolf!";
                break;
        }
        GameOverMenu.SetActive(true);
        currentMenu = "gameover";
        uiActive = true;
        FmodEvents.instance.isMusicSilenced = true;
        FmodEvents.instance.SwitchMusicState();
        GlobalEventManager.ToggleUI.Invoke(true);
    }

    public void LoadRequiredBoxSlots()
    {
        for (int box = 0; box < Boxes.Length; box++)
        {
            GameObject[] itemSlots = new GameObject[Boxes[box].requestedItems.Length];
            Boxes[box].requiredItemSlots =  new InventorySlot[Boxes[box].requestedItems.Length];
            for (int slot = 0; slot < Boxes[box].requestedItems.Length; slot++)
            {
                itemSlots[slot] = Instantiate(itemSlotPrefab, QuestMenu[(box)].transform);
                Boxes[box].requiredItemSlots[slot] = itemSlots[slot].GetComponent<InventorySlot>();
            }
        }
    }
}

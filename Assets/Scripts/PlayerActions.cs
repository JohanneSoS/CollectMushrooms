using System.Collections;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    
    public bool uiActive;
    private string hovering = "none";
    
    //Abilities
    public bool canSniff = true;
    private bool sniffActive = false;
    [SerializeField] public float sniffDuration;
    [SerializeField] private float sniffCooldown;

    private bool canSleep;
    
    //Items
    public bool isCollecting;
    
    //Box
    public int currentBox = 0;
    
    void Awake()
    {
        GlobalEventManager.ToggleUI.AddListener(ToggleUI);
        GlobalEventManager.OnDayStart.AddListener(DayStart);
        GlobalEventManager.OnEveningStart.AddListener(EveningStart);
        GlobalEventManager.OnPickItem.AddListener(ToggleIsCollecting);
        GlobalEventManager.EnterZone.AddListener(OnEnterZone);
        GlobalEventManager.ExitZone.AddListener(OnExitZone);
        GlobalEventManager.OnCompleteBoxQuest.AddListener(CountCurrentBox);
    }

    void Update()
    {
        if (uiActive)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            {
                switch (hovering)
                {
                    case "Box":
                        GlobalEventManager.InteractWithBox.Invoke(currentBox);
                        break;
                    case "NPC":
                        GlobalEventManager.OnInteractWithNPC.Invoke();
                        break;
                    case "SleepingPlace":
                        GlobalEventManager.ConfirmUI.Invoke();
                        break;
                    case "None":
                        GlobalEventManager.ConfirmUI.Invoke();
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (hovering != "NPC")
                {
                    GlobalEventManager.ResumeGame.Invoke();
                }
            }
        }
        else if (!uiActive)
        {
            switch (hovering)
            {
                case "SleepingPlace":
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                    {
                        if (canSleep)
                        {
                            GlobalEventManager.OpenSleepUI.Invoke();   
                        }
                        else if (!canSleep)
                        {
                            print("it's not the time to sleep!");
                        }
                        
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sniff();
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        GlobalEventManager.PauseGame.Invoke();
                    }
                    break;
                case "NPC":
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                    {
                        GlobalEventManager.OnInteractWithNPC.Invoke();
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sniff();
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        GlobalEventManager.PauseGame.Invoke();
                    }
                    break;
                case "Box":
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                    {
                        GlobalEventManager.OpenQuestUI.Invoke();
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sniff();
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        GlobalEventManager.PauseGame.Invoke();
                    }
                    break;
                case "Mushroom":
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GlobalEventManager.PickUpMushroom.Invoke();                    
                    } 
                    break;
                case "None":
                {
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                    {
                        UseSelectedItem();
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sniff();
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        GlobalEventManager.PauseGame.Invoke();
                    }
                    break;
                }
            }
        }
    }
    
    void OnEnterZone(string origin)
    {
        if (origin == "Box" || origin == "NPC" || origin == "SleepingPlace" || origin == "Mushroom")
        {
            hovering = origin;
        }
    }

    void OnExitZone(string origin)
    {
        if (hovering == origin || (hovering != "Box" && hovering != "NPC" && hovering != "SleepingPlace" && hovering !=  "Mushroom" && hovering !=  "None"))
        {
            hovering = "None";
        }
    }
    
    void ToggleUI(bool uiState)
    {
        uiActive = uiState;
    }

    void UseSelectedItem()
    {
        var selectedItem = InventoryManager.instance.GetSelectedItem(false);
        if (selectedItem != null)
        {
            if (selectedItem.canEat && playerStats.currentHunger < playerStats.maxHunger && !isCollecting)
            { 
                GlobalEventManager.HealHunger.Invoke(selectedItem.hungerAmount);
                Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
            } 
        }
    }
    
    private void Sniff()
    {
        if (sniffActive != true && canSniff)
        {
            GlobalEventManager.OnSniffing.Invoke();
            StartCoroutine(SniffDuration());
        }
    }

    IEnumerator SniffDuration()
    {
        canSniff = false;
        sniffActive = true;
        yield return new WaitForSeconds(sniffDuration);
        sniffActive = false;
        GlobalEventManager.OnSniffingEnd.Invoke();
        yield return new WaitForSeconds(sniffCooldown);
        canSniff = true;
    }

    void DayStart()
    {
        canSleep = false;
    }

    void EveningStart()
    {
        canSleep = true;
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

    private void CountCurrentBox()
    {
        currentBox++;
    }
    
}

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
    
    void Awake()
    {
        EventManager.ToggleUI.AddListener(ToggleUI);
        EventManager.OnDayStart.AddListener(DayStart);
        EventManager.OnEveningStart.AddListener(EveningStart);
        EventManager.OnPickItem.AddListener(ToggleIsCollecting);
        EventManager.EnterZone.AddListener(OnEnterZone);
        EventManager.ExitZone.AddListener(OnExitZone);
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
                        EventManager.InteractWithBox.Invoke();
                        break;
                    case "NPC":
                        EventManager.OnInteractWithNPC.Invoke();
                        break;
                    case "SleepingPlace":
                        EventManager.ConfirmUI.Invoke();
                        break;
                    case "None":
                        EventManager.ConfirmUI.Invoke();
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
                    EventManager.ResumeGame.Invoke();
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
                            EventManager.OpenSleepUI.Invoke();   
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
                        EventManager.PauseGame.Invoke();
                    }
                    break;
                case "NPC":
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                    {
                        EventManager.OnInteractWithNPC.Invoke();
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sniff();
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        EventManager.PauseGame.Invoke();
                    }
                    break;
                case "Box":
                    if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
                    {
                        EventManager.OpenQuestUI.Invoke();
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        Sniff();
                    }
                    else if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        EventManager.PauseGame.Invoke();
                    }
                    break;
                case "Mushroom":
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        EventManager.PickUpMushroom.Invoke();                    
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
                        EventManager.PauseGame.Invoke();
                    }
                    break;
                }
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Zone"))
        {
            hovering = other.tag;
        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (hovering == other.tag)
        {
            hovering = "none";
        }
    }*/

    void OnEnterZone(string origin)
    {
        hovering = origin;
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
        if (selectedItem.canEat && playerStats.currentHunger < playerStats.maxHunger && !isCollecting)
        { 
            EventManager.HealHunger.Invoke(selectedItem.hungerAmount);
            Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
        } 
        //StartCoroutine(CheckIfCanEat(selectedItem));
    }
    
    /*IEnumerator CheckIfCanEat(Item selectedItem)
    {
        yield return new WaitForSeconds(0.2f);
        if (selectedItem.canEat && playerStats.currentHunger < playerStats.maxHunger && !isCollecting)
        {
            EventManager.HealHunger.Invoke(selectedItem.hungerAmount);
            Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
        }
    }*/
    
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
        yield return new WaitForSeconds(sniffDuration);
        sniffActive = false;
        EventManager.OnSniffingEnd.Invoke();
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
    
}

using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuestRecipient : MonoBehaviour
{
    [SerializeField] private Item[] requestedItems;
    [SerializeField] private Sprite[] boxStates;
    //public static List<Item> requestedItem;
    private static List <RequiredItem> requestedUIItems;

    private static List<Item> currentItemsInBox = new();
    //private Item recieveItem;
    private int totalRequestedItemCount;
    private int deliveredCount = 0;
    private float amountForSingleSection;
    private int roundedAmountForSingleSection;

    private int boxStateIndex = 0;

    private bool playerHovering = false;
    private float interactionTimer = 2f; //Add Cooldown
    
    [Header("UI Logic")]
    public InventorySlot [] requiredItemSlots;
    public RequiredItem requiredItemPrefab;
    public GameObject panel;
    private bool uiActive;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = true;
        }
    }

    private void Start()
    {
        requestedUIItems = new List<RequiredItem>();
        for (int i = 0; i < requestedItems.Length; i++)
        {
            var slot = requiredItemSlots[i];
            var newRequestedUIItem = SpawnNewItem(requestedItems[i], slot, true);
            requestedUIItems.Add(newRequestedUIItem);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHovering && !uiActive)
        {
            StartCoroutine(OpenUI());
        }

        if (Input.GetKeyDown(KeyCode.E) && uiActive)
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && uiActive)
        {
            CloseUI();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = false;
        }
    }

    IEnumerator OpenUI()
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        uiActive = true;
    }

    private void CloseUI()
    {
        panel.SetActive(false);
        uiActive = false;
    }
    
    private void Interact()
    {
        var selectedItem = InventoryManager.instance.GetSelectedItem(false);
        var matchingRequestedUIItem = requestedUIItems.FirstOrDefault(ui => ui.requiredItem == selectedItem && !ui.delivered);

        if (matchingRequestedUIItem != null)
        {
            UseSelectedItem();
            currentItemsInBox.Add(selectedItem);
            matchingRequestedUIItem.DeliverItem();

            //var deliveredCount = requestedUIItems.Count(ui => ui.delivered);
            deliveredCount = requestedUIItems.Count(ui => ui.delivered);
            var deliverProgress = (float)requestedUIItems.Count / deliveredCount;
            
            if (deliveredCount == requestedUIItems.Count)
            {
                //QuestManager.instance.QuestFinished();
                EventManager.OnQuestFinished.Invoke();
                Debug.Log("Alle Items sind da");
                CloseUI();
                //Destroy(gameObject, 2);
            }
        }
    }
    
    public void UseSelectedItem()
    {
        Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
    }

    private void CheckBoxState()
    {
        switch (boxStateIndex)
        {
            case 0: GetComponent<SpriteRenderer>().sprite = boxStates[0]; break;
            case 1: GetComponent<SpriteRenderer>().sprite = boxStates[1]; break;
            case 2: GetComponent<SpriteRenderer>().sprite = boxStates[2]; break;
            case 3: GetComponent<SpriteRenderer>().sprite = boxStates[3]; break;
        }
    }

    private void ClearBox()
    {
        for (int i = 0; i < requiredItemSlots.Length; i++)
        {
            InventorySlot slot = requiredItemSlots[i];
            RequiredItem itemInSlot = slot.GetComponentInChildren<RequiredItem>();
            Destroy(itemInSlot.gameObject);
        }
    }
    
    private bool AddItem(Item item)
    {
        for (int i = 0; i < requiredItemSlots.Length; i++)
        {
            InventorySlot slot = requiredItemSlots[i];
            RequiredItem itemInSlot = slot.GetComponentInChildren<RequiredItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot, false);
                return true;
            }
        }
        return false;
    }

    private bool AddSilhuetteItem(Item item)
    {
        for (int i = 0; i < requiredItemSlots.Length; i++)
        {
            InventorySlot slot = requiredItemSlots[i];
            RequiredItem itemInSlot = slot.GetComponentInChildren<RequiredItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot, true);
                return true;
            }
        }
        return false;
    }

    RequiredItem SpawnNewItem(Item item, InventorySlot slot, bool silhuetteState)
    {
        RequiredItem newItem = Instantiate(requiredItemPrefab, slot.transform);
        newItem.ChangeSilhouetteState(silhuetteState);
        newItem.InitialiseItem(item);
        return newItem;
    }
}

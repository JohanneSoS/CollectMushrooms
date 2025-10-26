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
    private float amountForSingleSection;
    private int roundedAmountForSingleSection;

    private int boxStateIndex = 0;
    //private Item[] currentItems;

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
        
        // totalRequestedItemCount = currentRequestedItems.Count;
        // amountForSingleSection = (1.0f * totalRequestedItemCount / 3);
        // roundedAmountForSingleSection = Mathf.RoundToInt(amountForSingleSection);
        //ClearBox();
        //ShowRequiredItems();
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

            var deliveredCount = requestedUIItems.Count(ui => ui.delivered);
            var deliverProgress = (float)requestedUIItems.Count / deliveredCount;
            
            if (deliveredCount == requestedUIItems.Count)
            {
                EventManager.OnQuestFinished.Invoke();
            }
        }
        
        // if (currentRequestedItems.Contains(InventoryManager.instance.GetSelectedItem(false)))
        // {
        //     Item itemSelectedinInventory = InventoryManager.instance.GetSelectedItem(false);
        //     UseSelectedItem();
        //     //currentRequestedItems.Remove(itemSelectedinInventory);
        //     currentItemsInBox.Add(itemSelectedinInventory);
        //     //ClearBox();
        //     ShowRequiredItems();
        //     //Logic to check quest state
        //
        //     if (currentRequestedItems.Count == 0)
        //     {
        //         boxStateIndex = 3;
        //         CheckBoxState();
        //         EventManager.OnQuestFinished.Invoke();
        //         print("Quest finished");
        //     }
        //     else if (currentRequestedItems.Count == roundedAmountForSingleSection)
        //     {
        //         boxStateIndex = 2;
        //         CheckBoxState();
        //     }
        //     else if (currentRequestedItems.Count == (roundedAmountForSingleSection*2))
        //     {
        //         boxStateIndex = 1;
        //         CheckBoxState();
        //     }
        // }
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
    
    /*public Item RemoveItemFromBox()
    {
        InventorySlot slot = requiredItemSlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        }
        return null;
    }*/
}

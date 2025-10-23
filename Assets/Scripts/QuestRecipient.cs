using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuestRecipient : MonoBehaviour
{
    [SerializeField] private Item[] requestedItems;
    [SerializeField] private Sprite[] boxStates;
    //public static List<Item> requestedItem;
    private static List <Item> currentRequestedItems;
    private int totalRequestedItemCount;
    private float amountForSingleSection;
    private int roundedAmountForSingleSection;

    private int boxStateIndex = 0;
    //private Item[] currentItems;

    private bool playerHovering = false;
    private float interactionTimer = 2f; //Add Cooldown
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = true;
        }
    }

    private void Start()
    {
        currentRequestedItems = requestedItems.ToList();
        totalRequestedItemCount = currentRequestedItems.Count;
        amountForSingleSection = (1.0f * totalRequestedItemCount / 3);
        roundedAmountForSingleSection = Mathf.RoundToInt(amountForSingleSection);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHovering)
        {
            Interact();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = false;
        }
    }
    
    private void Interact()
    {
        if (currentRequestedItems.Contains(InventoryManager.instance.GetSelectedItem(false)))
        {
            UseSelectedItem();
            currentRequestedItems.Remove(InventoryManager.instance.GetSelectedItem(false));
            
            //Logic to check quest state

            if (currentRequestedItems.Count == 0)
            {
                boxStateIndex = 3;
                CheckBoxState();
                EventManager.OnQuestFinished.Invoke();
                print("Quest finished");
            }
            else if (currentRequestedItems.Count == roundedAmountForSingleSection)
            {
                boxStateIndex = 2;
                CheckBoxState();
            }
            else if (currentRequestedItems.Count == (roundedAmountForSingleSection*2))
            {
                boxStateIndex = 1;
                CheckBoxState();
            }
        }
    }
    
    public void UseSelectedItem()
    {
        Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
    }

    // von 0-25;25-50;50-75;75-100
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
}

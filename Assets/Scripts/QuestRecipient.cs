using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuestRecipient : MonoBehaviour
{
    [SerializeField] private Item[] requestedItems;
    //public static List<Item> requestedItem;
    private static List <Item> currentRequestedItems;
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

            if (currentRequestedItems.Count == 0)
            {
                QuestFinished();
            }
        }
    }
    
    public void UseSelectedItem()
    {
        Item recieveItem = InventoryManager.instance.GetSelectedItem(true);
    }

    private void QuestFinished()
    {
        print("Quest finished");
        Destroy(gameObject);
    }
}

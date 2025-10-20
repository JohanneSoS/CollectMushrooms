using UnityEngine;
using UnityEngine.Splines;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public Item[] itemsToPickup;
    public int maxStacks;
    
    public void Awake()
    {
        instance = this;
    }
    public void PickupItem(int id)
    {
        AddItem(itemsToPickup[id]);
    }
    
    public bool AddItem(Item item)
    {
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStacks)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}

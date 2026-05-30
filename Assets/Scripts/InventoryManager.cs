using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public Item[] itemsToPickup;
    public int maxStacks;

    private int mouseWheelDirection;
    private bool itemSlotChangeCD;
    private int selectedSlot = -1;
   
    public void Awake()
    {
        instance = this;
        GlobalEventManager.OnRespawn.AddListener(ClearInventory);
    }

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out  int number);
            if (isNumber && number > 0 && number < inventorySlots.Length)
            {
                ChangeSelectedSlot(number -1);
            }
            else if (isNumber && number == 0)
            {
                ChangeSelectedSlot(9);
            }
            else if (Input.inputString == "ß" || Input.inputString == "." || Input.inputString == "-")
            {
                ChangeSelectedSlot(10);
            }
            
        } //this is the input
    }

    private void FixedUpdate()
    {
        if ((Input.GetAxis("Mouse ScrollWheel")* 10) != 0)
        {
            float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel") * 10;
            if (mouseWheelInput > 0)
            {
                mouseWheelDirection = -1;
            }
            else if (mouseWheelInput < 0)
            {
                mouseWheelDirection = 1;
            }
            ChangeSelectedSlot((selectedSlot + mouseWheelDirection));
        }
        else
        {
            mouseWheelDirection = 0;
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (newValue >= 10)
        {
            newValue = 10;
        }
        else if (newValue <= 0)
        {
            newValue = 0;
        }
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Unselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        GlobalEventManager.UpdateItemDiscription.Invoke(GetSelectedItem(false));
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
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
                GlobalEventManager.OnPickItem.Invoke();
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
                GlobalEventManager.OnPickItem.Invoke();
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

    void ClearInventory()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                itemInSlot.Destroy();
            }
        }
    }
}

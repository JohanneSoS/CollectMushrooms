using TMPro;
using UnityEngine;

public class ItemDiscription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI discriptionText;
    private Item itemInSlot;

    void Awake()
    {
        GlobalEventManager.UpdateItemDiscription.AddListener(UpdateSelectedItem);
    }
    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
    
    private void UpdateSelectedItem(Item item)
    {
        if (item != null)
        {
            itemInSlot = item;
            discriptionText.text = "<color=#6900a6>" + itemInSlot.itemName + "</color >" + "\n" + "<color=#502803>"+ itemInSlot.type +"</color >" + "\n" + "<color=#902B2B>" + "Health: "  + "</color >" +itemInSlot.healAmount +"\n" + "<color=#846F2C>" + "Hunger: " + "</color >" +itemInSlot.hungerAmount +"\n" + "<color=#226993>" + "Exhaustion: " + "</color>" +itemInSlot.exhaustAmount;
        }
        else
        {
            itemInSlot = null;
            discriptionText.text = "No item selected";
        }
    }
}

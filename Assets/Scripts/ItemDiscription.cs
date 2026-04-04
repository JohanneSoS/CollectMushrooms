using TMPro;
using UnityEngine;

public class ItemDiscription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI discriptionText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI hungerText;
    [SerializeField] private TextMeshProUGUI exhaustionText;
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
            discriptionText.text = "<color=#6900a6>" + itemInSlot.itemName + "</color >" + "\n" + "<color=#502803>"+ itemInSlot.type +"</color >";
            
            if (itemInSlot.healAmount >= 0)
            {
                healthText.text = "<color=#356614>" + itemInSlot.healAmount + "</color >";
            }
            else
            {
                healthText.text = "<color=#902B2B>" + itemInSlot.healAmount + "</color >";
            }

            if (itemInSlot.hungerAmount >= 0)
            {
                hungerText.text = "<color=#356614>" + itemInSlot.hungerAmount + "</color >";
            }
            else
            {
                hungerText.text = "<color=#902B2B>" + itemInSlot.hungerAmount + "</color >";
            }
            
            if (itemInSlot.exhaustAmount >= 0)
            {
                exhaustionText.text = "<color=#356614>" + itemInSlot.exhaustAmount + "</color >";
            }
            else
            {
                exhaustionText.text = "<color=#902B2B>" + itemInSlot.exhaustAmount + "</color >";
            }
        }
        else
        {
            itemInSlot = null;
            discriptionText.text = "No item selected";
        }
    }
}

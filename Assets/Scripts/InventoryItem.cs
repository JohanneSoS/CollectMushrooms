using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour
{
    public Item item;
    public int count;
    public Text countText;
    public Image image;

    void Start()
    {
        InitialiseItem(item);
    }

    public void RefreshCount ()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.sprite;
        RefreshCount();
    }

}

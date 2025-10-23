using UnityEngine;
using UnityEngine.UI;

public class RequiredItem : MonoBehaviour
{
    public Item requiredItem;
    public Image image;
    
    void Start()
    {
        image = GetComponent<Image>();
        InitialiseItem(requiredItem);
    }

    public void InitialiseItem(Item newItem)
    {
        requiredItem = newItem;
        image.sprite = newItem.sprite;
    }
    

}

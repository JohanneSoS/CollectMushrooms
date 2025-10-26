using UnityEngine;
using UnityEngine.UI;

public class RequiredItem : MonoBehaviour
{
    public Item requiredItem;
    public Image image;
    public bool delivered;
    
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

    public void DeliverItem()
    {
        delivered = true;
        ChangeSilhouetteState(false);
    }
    
    [ContextMenu("ChangeSilhoutte")]
    public void ChangeSilhouetteState(bool silhouetteState)
    {
        Debug.Log($"new silhoutte: {silhouetteState}");
        if (silhouetteState)
        {
            image.color = new Color(0, 0, 0, 255);
        }
        else
        {
            image.color = new Color(255, 255, 255, 255);
        }
    }
}

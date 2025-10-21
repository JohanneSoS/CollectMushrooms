using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public Color selectedColor;
    public Color unselectedColor;

    private void Awake()
    {
        Unselect();
    }

    public void Select()
    {
        image.color = selectedColor;    
    }

    public void Unselect()
    {
        image.color = unselectedColor;
    }
    
}

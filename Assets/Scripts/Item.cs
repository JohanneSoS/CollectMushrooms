using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public ItemType type;
    public Vector2Int range = new Vector2Int(5, 4);
    public bool canEat;
    public bool usable;
    public int hungerAmount;
    public int healAmount;
    public int exhaustAmount;
    public float movementMultiplier;
    public float effectDuration;
    
    [Header("Only UI")]
    public bool stackable = true;

    public string itemName;
    
    [Header("Both")]
    public Sprite sprite;

    public enum ItemType
    {
        Mushroom,
        QuestItem,
        Tool,
        SpecialItem
    }
}

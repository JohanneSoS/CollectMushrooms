using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only Gameplay")]
    public TileBase tile;
    public ItemType type;
    //public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public bool canSell = true;
    public float sellPrice;
    public float buyPrice;
    
    [Header("Only UI")]
    public bool stackable = true;
    
    [Header("Both")]
    public Sprite sprite;

    public enum ItemType
    {
        Mushroom,
        BuildingBlock,
        Tool,
    }

    /*public enum ActionType
    {
        
    }*/
}

using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D col;

    [SerializeField] private Item item;

    private bool playerHovering = false;

    private int age = 0;

    void Awake()
    {
        GlobalEventManager.OnDayStart.AddListener(StartDay);
        GlobalEventManager.OnNightStart.AddListener(StartNight);
        GlobalEventManager.PickUpMushroom.AddListener(PickUp);
        col = GetComponent<BoxCollider2D>();
        age = 1;
    }

    private void StartDay()
    {
        UpdateAge();
    }

    private void StartNight()
    {
        UpdateAge();
    }
    void UpdateAge()
    {
        if (item.type == Item.ItemType.Mushroom && age >= 2){
            Destroy(gameObject);
        }
        age += 1;
    }
    
    public void Initialize(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHovering = false;
        }
    }
    
    private void PickUp()
    {
        if (playerHovering)
        {
            bool canAdd = InventoryManager.instance.AddItem(item);
            if (canAdd)
            {
                Destroy(col);
                Destroy(gameObject);
            }
        }
    }
}

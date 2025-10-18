using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D collider;

    [SerializeField] private Item item;

    private bool playerHovering = false;

    public void Initialize(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.sprite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHovering)
        {
            PickUp();
        }
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
        bool canAdd = InventoryManager.instance.AddItem(item);
        if (canAdd)
        {
            Destroy(collider);
            Destroy(gameObject);
        }
    }
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine()
        }
    }*/
}

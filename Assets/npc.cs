using System;
using UnityEngine;

public class npc : MonoBehaviour
{
    private Collider2D col;
    private bool playerHovering = false;
    [SerializeField] private DialogueCharacter charType;
    private string charName;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        charName = charType.name;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerHovering)
        {
            EventManager.OnInteractWithNPC.Invoke(charName);
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

    private void Interact()
    {

    }
}

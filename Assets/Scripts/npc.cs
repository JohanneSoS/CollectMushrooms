using System;
using UnityEngine;

public class npc : MonoBehaviour
{
    private Collider2D col;
    [SerializeField] private DialogueCharacter charType;
    private string charName;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        charName = charType.name;
    }
}

public enum NPC
{
    Racoon,
    Beaver,
    Jay,
    Boar
}

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideLayer : MonoBehaviour
{
    [SerializeField] private float fullVisibility;
    [SerializeField] private float transparency;
    [SerializeField] private bool hideBySneeze;

    private bool playerHovering = false;

    private void Start()
    {
        Show();
    }
    
    void Awake()
    {
        EventManager.OnSniffing.AddListener(Hide);
        EventManager.OnSniffingEnd.AddListener(Show);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Hide();
            playerHovering = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Show();
            playerHovering = false;
        }
    }

    public void Hide()
    {
        var color = new Color(1.0f, 1.0f, 1.0f, transparency);
        this.GetComponent<Tilemap>().color = color;
    }

    public void Show()
    {
        var color = new Color(1.0f, 1.0f, 1.0f, fullVisibility);
        this.GetComponent<Tilemap>().color = color;
    }
}

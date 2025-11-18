using UnityEngine;

public class ZoomArea : MonoBehaviour
{
    [SerializeField] private float zoomValue;
    private Collider2D col2D;
    private bool playerHovering = false;

    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerHovering)
            {
                EventManager.OnChangeZoomForArea.Invoke(zoomValue);
                playerHovering = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHovering)
            {
                EventManager.OnResetZoom.Invoke();
                playerHovering = false;
            }
        }
    }

}

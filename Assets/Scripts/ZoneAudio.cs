using UnityEngine;

public class ZoneAudio : MonoBehaviour
{

    private Collider2D col;
    [SerializeField] private AudioZone zone;
    [SerializeField] private ZoneOrigin zoneOrigin;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventManager.EnterAudioZone.Invoke(zone, zoneOrigin);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventManager.ExitAudioZone.Invoke(zone, zoneOrigin);
        }
    }
}

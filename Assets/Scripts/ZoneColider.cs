 using System;
using UnityEngine;

public class ZoneColider : MonoBehaviour
{
    private Collider2D Col;

    private void Awake()
    {
        Col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (gameObject.tag != "AudioZone")
            {
                EventManager.EnterZone.Invoke(this.gameObject.tag);
                Debug.Log(this.gameObject.tag);
            }
            else if (gameObject.tag == "AudioZone")
            {
                EventManager.EnterZone.Invoke(this.gameObject.name);
                //New Event for AudioZone-Enter
                Debug.Log(this.gameObject.name);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        {
            if (gameObject.tag != "AudioZone")
            {
                EventManager.ExitZone.Invoke(this.gameObject.tag);
                Debug.Log(this.gameObject.tag);
            }
            else if (gameObject.tag == "AudioZone")
            {
                EventManager.ExitZone.Invoke(this.gameObject.name);
                Debug.Log(this.gameObject.name);
            }
        }
    }
}

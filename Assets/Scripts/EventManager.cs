using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    //public static EventManager instance;

    public static UnityEvent OnWalking = new UnityEvent();
    public static FloatEvent OnSniffing = new FloatEvent();
    public static UnityEvent OnPickItem = new UnityEvent();
    public static UnityEvent OnGiveItem = new UnityEvent();
    
}
    public class FloatEvent : UnityEvent<float> { }


using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static UnityEvent OnWalkingStart = new UnityEvent();
    public static UnityEvent OnWalkingStop = new UnityEvent();
    public static UnityEvent OnSniffing = new UnityEvent();
    public static UnityEvent OnSniffingEnd = new UnityEvent();
    public static UnityEvent OnPickItem = new UnityEvent();
    public static UnityEvent OnGiveItem = new UnityEvent();

    public static UnityEvent OnDayStart = new UnityEvent();
    public static UnityEvent OnNightStart = new UnityEvent();

    public static UnityEvent OnQuestFinished = new UnityEvent();
}
    public class FloatEvent : UnityEvent<float> { }


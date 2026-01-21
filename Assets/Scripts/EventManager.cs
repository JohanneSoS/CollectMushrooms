using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static BoolEvent ToggleUI = new BoolEvent();
    public static UnityEvent OpenSleepUI = new UnityEvent();
    public static UnityEvent OpenQuestUI = new UnityEvent();
    public static UnityEvent CloseQuestUI = new UnityEvent();
    public static UnityEvent PauseGame = new UnityEvent();
    public static UnityEvent ResumeGame = new UnityEvent();
    public static UnityEvent ConfirmUI = new UnityEvent();
    
    public static UnityEvent OnWalkingStart = new UnityEvent();
    public static UnityEvent OnWalkingStop = new UnityEvent();
    public static UnityEvent OnSniffing = new UnityEvent();
    public static UnityEvent OnSniffingEnd = new UnityEvent();
    public static UnityEvent OnPickItem = new UnityEvent();
    public static UnityEvent OnGiveItem = new UnityEvent();
    public static UnityEvent ActivateItem = new UnityEvent();
    public static UnityEvent PickUpMushroom = new UnityEvent();

    public static UnityEvent OnDayStart = new UnityEvent();
    public static UnityEvent OnEveningStart = new UnityEvent();
    public static UnityEvent OnNightStart = new UnityEvent();
    public static UnityEvent OnSkipToDay = new UnityEvent();

    public static UnityEvent OnQuestFinished = new UnityEvent();
    public static UnityEvent OnBaseUpgrade = new UnityEvent();

    /*public static UnityEvent OnFirstQuestComplete = new UnityEvent();
    public static UnityEvent OnSecondQuestComplete = new UnityEvent();
    public static UnityEvent OnThirdQuestComplete = new UnityEvent();*/

    public static FloatEvent OnChangeZoomForArea = new FloatEvent();
    public static UnityEvent OnResetZoom = new UnityEvent();

    public static UnityEvent OnChasing = new UnityEvent();
    
    [Header("Combat")]
    public static IntEvent ApplyDamage = new IntEvent();
    public static IntEvent ApplyHeal = new IntEvent();
    public static IntEvent ApplyHunger = new IntEvent();
    public static IntEvent HealHunger = new IntEvent();
    public static IntEvent ApplyExhaustion = new IntEvent();
    public static UnityEvent ResetExhaustion = new UnityEvent();
    
    public static IntEvent UpdateHealthBar = new IntEvent();
    public static IntEvent UpdateHungerBar = new IntEvent();
    public static IntEvent UpdateExhaustionBar = new IntEvent();

    [Header ("Quest and Dialogue")]
    public static IntEvent OnStartQuest = new IntEvent();
    public static IntEvent OnAdvanceQuest = new IntEvent();
    public static UnityEvent OnItemsDelivered = new UnityEvent();
    public static IntEvent OnCompleteQuest = new IntEvent();
    public static UnityEvent OnInteractWithNPC = new UnityEvent();
    public static UnityEvent InteractWithBox = new UnityEvent();
    public static UnityEvent OnDialogueStart = new UnityEvent();
    //public static UnityEvent OnDialogueEnd = new UnityEvent();

}
public class FloatEvent : UnityEvent<float> { }
public class IntEvent : UnityEvent<int> { }
public class StringEvent : UnityEvent<string> { }
public class BoolEvent : UnityEvent<bool> { }


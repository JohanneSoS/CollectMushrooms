using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    public static BoolEvent ToggleUI = new BoolEvent();
    public static UnityEvent OpenSleepUI = new UnityEvent();
    public static UnityEvent OpenQuestUI = new UnityEvent();
    public static UnityEvent CloseQuestUI = new UnityEvent();
    public static UnityEvent PauseGame = new UnityEvent();
    public static UnityEvent ResumeGame = new UnityEvent();
    public static UnityEvent ConfirmUI = new UnityEvent();
    
    public static UnityEvent OnMovement = new UnityEvent();
    public static UnityEvent OnWalkingStart = new UnityEvent();
    public static UnityEvent OnWalkingStop = new UnityEvent();
    public static UnityEvent OnSniffing = new UnityEvent();
    public static UnityEvent OnSniffingEnd = new UnityEvent();
    public static UnityEvent OnPickItem = new UnityEvent();
    public static UnityEvent OnEatItem = new UnityEvent();
    public static UnityEvent OnGiveItem = new UnityEvent();
    public static UnityEvent ActivateItem = new UnityEvent();
    public static UnityEvent PickUpMushroom = new UnityEvent();
    public static ItemEvent UpdateItemDiscription = new ItemEvent();

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
    
    //Zone Management
    public static StringEvent EnterZone = new StringEvent();
    public static StringEvent ExitZone = new StringEvent();
    public static AudioZoneEvent EnterAudioZone = new AudioZoneEvent();
    public static AudioZoneEvent ExitAudioZone = new AudioZoneEvent();
    
    //Combat
    public static IntEvent ApplyDamage = new IntEvent();
    public static IntEvent ApplyHeal = new IntEvent();
    public static IntEvent ApplyHunger = new IntEvent();
    public static IntEvent HealHunger = new IntEvent();
    public static IntEvent ApplyExhaustion = new IntEvent();
    public static UnityEvent ResetExhaustion = new UnityEvent();
    public static StringEvent OnGameOver = new StringEvent();
    public static UnityEvent OnRespawn = new UnityEvent();
    public static FloatFloatEvent ChangeMovementSpeed = new FloatFloatEvent();
    
    public static IntEvent UpdateHealthBar = new IntEvent();
    public static IntEvent UpdateHungerBar = new IntEvent();
    public static IntEvent UpdateExhaustionBar = new IntEvent();

    //Quest and Dialog
    public static IntEvent OnStartQuest = new IntEvent();
    public static IntEvent OnAdvanceQuest = new IntEvent();
    public static UnityEvent OnItemsDelivered = new UnityEvent();
    public static IntEvent OnCompleteQuest = new IntEvent();
    public static UnityEvent OnCompleteBoxQuest = new UnityEvent();
    public static UnityEvent OnInteractWithNPC = new UnityEvent();
    public static IntEvent InteractWithBox = new IntEvent();
    public static UnityEvent OnDialogueStart = new UnityEvent();
    //public static UnityEvent OnDialogueEnd = new UnityEvent();

}
public class FloatEvent : UnityEvent<float> { }
public class IntEvent : UnityEvent<int> { }
public class StringEvent : UnityEvent<string> { }
public class AudioZoneEvent : UnityEvent<AudioZone, ZoneOrigin> {}
public class BoolEvent : UnityEvent<bool> { }

public class ItemEvent : UnityEvent<Item> { }

public class FloatFloatEvent : UnityEvent<float, float> { }

public class Collider2DEvent : UnityEvent<Collider2D> { }


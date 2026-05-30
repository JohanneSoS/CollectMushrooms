using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEventManager
{
    //GameManagement
    public static BoolEvent GamePaused = new BoolEvent();
    public static UnityEvent PauseGame = new UnityEvent();
    public static UnityEvent ResumeGame = new UnityEvent();
    public static StringEvent OnGameOver = new StringEvent();
    //TimeSystem
    public static UnityEvent OnDayStart = new UnityEvent();
    public static UnityEvent OnEveningStart = new UnityEvent();
    public static UnityEvent OnNightStart = new UnityEvent();
    public static UnityEvent OnSkipToDay = new UnityEvent();
    //UI
    public static BoolEvent ToggleUI = new BoolEvent();
    public static UnityEvent OpenSleepUI = new UnityEvent();
    public static UnityEvent OpenQuestUI = new UnityEvent();
    public static UnityEvent CloseQuestUI = new UnityEvent();
    public static UnityEvent ConfirmUI = new UnityEvent();
    public static IntEvent UpdateHealthBar = new IntEvent();
    public static IntEvent UpdateHungerBar = new IntEvent();
    public static IntEvent UpdateExhaustionBar = new IntEvent();
    //Movement
    public static UnityEvent OnMovement = new UnityEvent();
    public static UnityEvent OnWalkingStart = new UnityEvent();
    public static UnityEvent OnWalkingStop = new UnityEvent();
    public static FloatFloatEvent ChangeMovementSpeed = new FloatFloatEvent();
    //Actions
    public static UnityEvent OnSniffing = new UnityEvent();
    public static UnityEvent OnSniffingEnd = new UnityEvent();
    //ItemLogic
    public static UnityEvent OnPickItem = new UnityEvent();
    public static UnityEvent OnEatItem = new UnityEvent();
    public static UnityEvent OnGiveItem = new UnityEvent();
    public static UnityEvent PickUpMushroom = new UnityEvent();
    public static ItemEvent UpdateItemDiscription = new ItemEvent();
    //PlayerStats
    public static IntEvent ApplyDamage = new IntEvent();
    public static IntEvent ApplyHeal = new IntEvent();
    public static IntEvent ApplyHunger = new IntEvent();
    public static IntEvent HealHunger = new IntEvent();
    public static IntEvent ApplyExhaustion = new IntEvent();
    public static UnityEvent ResetExhaustion = new UnityEvent();
    //QuestLogic
    public static IntEvent OnStartQuest = new IntEvent();    
    public static IntEvent OnAdvanceQuest = new IntEvent();
    public static UnityEvent OnItemsDelivered = new UnityEvent();
    public static IntEvent OnCompleteQuest = new IntEvent();
    public static UnityEvent OnCompleteBoxQuest = new UnityEvent();
    public static UnityEvent OnInteractWithNPC = new UnityEvent();
    public static IntEvent InteractWithBox = new IntEvent();
    //Dialog
    public static UnityEvent OnDialogueStart = new UnityEvent();
    //Environment
    public static UnityEvent BaseUpgrade = new UnityEvent();
    public static UnityEvent OnResetZoom = new UnityEvent();
    //Zone Management
    public static FloatEvent OnChangeZoomForArea = new FloatEvent();
    public static StringEvent EnterZone = new StringEvent();
    public static StringEvent ExitZone = new StringEvent();
    public static AudioZoneEvent EnterAudioZone = new AudioZoneEvent();
    public static AudioZoneEvent ExitAudioZone = new AudioZoneEvent();
    //Enemy
    public static UnityEvent OnChasing = new UnityEvent();
    //Combat
    public static UnityEvent OnRespawn = new UnityEvent();
}
public class FloatEvent : UnityEvent<float> { }
public class IntEvent : UnityEvent<int> { }
public class StringEvent : UnityEvent<string> { }
public class AudioZoneEvent : UnityEvent<AudioZone, ZoneOrigin> {}
public class BoolEvent : UnityEvent<bool> { }

public class ItemEvent : UnityEvent<Item> { }

public class FloatFloatEvent : UnityEvent<float, float> { }


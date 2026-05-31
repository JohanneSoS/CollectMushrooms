using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public int questCount = 0;
    public int questStep = 0;

    public GameObject[] questBoxes;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject[] npcs;
    [SerializeField] public Quest[] quests;
    public GameObject[] npcQuestPos;
    private Dictionary<int, Quest> questDict = new Dictionary<int, Quest>();
    private Dictionary<NPC, GameObject> npcDict = new Dictionary<NPC, GameObject>();
    private Dictionary<int, GameObject> boxDict = new Dictionary<int, GameObject>();
    
    [SerializeField] private UIManager uIManager;

    void Awake()
    {
        instance = this;

        for (int i = 0; i < quests.Length; i++)
        {
            questDict[i] = quests[i];
        }

        foreach (GameObject box in questBoxes)
        {
            int id = box.GetComponent<QuestRecipient>().questID;
            boxDict[id] = box;
        }
        
        npcDict.Add(NPC.Racoon, npcs[0]);
        npcDict.Add(NPC.Beaver, npcs[1]);
        npcDict.Add(NPC.Jay, npcs[2]);
        npcDict.Add(NPC.Boar, npcs[3]);
        
        GlobalEventManager.OnStartQuest.AddListener(StartQuest);
        GlobalEventManager.OnAdvanceQuest.AddListener(AdvanceQuest);
        GlobalEventManager.OnCompleteQuest.AddListener(CompleteQuest);
        GlobalEventManager.OnInteractWithNPC.AddListener(InteractWithNPC);
        GlobalEventManager.OnItemsDelivered.AddListener(OnItemsDelivered);
    }
    void Start()
    {
        GlobalEventManager.OnStartQuest.Invoke(questCount);
        questBoxes[0].SetActive(false);
    }

    void InteractWithNPC()
    {
        if (!dialogueManager.dialogueIsShowing)
        {
            if (!dialogueManager.currentDialogueRead)
            {
                GlobalEventManager.OnAdvanceQuest.Invoke(questCount);
            }
            else if (dialogueManager.currentDialogueRead)
            {
                dialogueManager.RepeatLastLine(questCount);
            }
        }
        else if (dialogueManager.dialogueIsShowing)
        {
            if (!dialogueManager.currentDialogueRead)
            {
                dialogueManager.ProgressQuestUntilFinish(questCount);
            }
            else if (dialogueManager.currentDialogueRead)
            {
                dialogueManager.HideDialogue();
            }
        }
    }

    void StartQuest(int questID)
    {
        questStep = 1;
        UpdateNPCLocations();
        switch (questID)
        {
            case 6: 
                FmodEvents.instance._baseMusicInstance.setParameterByName("Sections", 1);
                break;
            case 10:
                FmodEvents.instance._baseMusicInstance.setParameterByName("Sections", 2);
                break;
            case 14:
                FmodEvents.instance._baseMusicInstance.setParameterByName("Sections", 3);
                break;
        }
    }

    void AdvanceQuest(int questID)
    {
        questStep++;
        switch (questStep)
        {
            case 2:
                //StartDialog
                GlobalEventManager.OnDialogueStart.Invoke();
                FmodEvents.instance.PlayOneShot(FmodEvents.instance.firstQuestConvo);
               return;
            case 3:
                //FinishDialogue, EnableBox
                if (quests[questCount].questType == QuestType.OnlyDialogue)
                {
                    GlobalEventManager.OnCompleteQuest.Invoke(questCount);
                    FmodEvents.instance.PlayOneShot(FmodEvents.instance.finishQuest);
                }
                else
                {
                    EnableQuestBox();
                    FmodEvents.instance.PlayOneShot(FmodEvents.instance.startQuest);
                }
                return;
            case 4:
                DisableQuestBox();
                FmodEvents.instance.PlayOneShot(FmodEvents.instance.finishDelivery);
                return;
            case 5:
                GlobalEventManager.OnDialogueStart.Invoke();
                return;
            case 6:
                GlobalEventManager.OnCompleteQuest.Invoke(questCount);
                FmodEvents.instance.PlayOneShot(FmodEvents.instance.finishQuest);
                GlobalEventManager.OnCompleteBoxQuest.Invoke();
                return;
        }
    }

    void OnItemsDelivered()
    {
        AdvanceQuest(questCount);
    }
    public void CompleteQuest(int questID)
    {
        questStep = 0;
        questCount++;
        GlobalEventManager.OnStartQuest.Invoke(questCount);
    }

    void EnableQuestBox()
    {
        if (quests[questCount].questType != QuestType.OnlyDialogue)
        {
            for (int i = 0; i < questBoxes.Length; i++)
            {
                questBoxes[i].SetActive(false); 
            }
            boxDict[questCount].transform.position = npcQuestPos[(questCount)].transform.position + new Vector3(-0.5f, -0.7f, 0);
            boxDict[questCount].SetActive(true);
            uIManager.activeBox = boxDict[questCount].GetComponent<QuestRecipient>().boxID;
        }
    }

    void DisableQuestBox()
    {   
        boxDict[questCount].SetActive(false);
    }

    void UpdateNPCLocations()
    {
        Vector3 npcNewPos;
        if (questCount < questDict.Count)
        {
            foreach (GameObject npc in npcDict.Values)
            {
                npc.SetActive(false);
            }
            npcDict[quests[questCount].npcType].SetActive(true);
            npcNewPos = npcQuestPos[(questCount)].transform.position;
            npcDict[quests[questCount].npcType].transform.position = npcNewPos;
            RuntimeManager.DetachInstanceFromGameObject(FmodEvents.instance._npcMusicInstance);
            RuntimeManager.AttachInstanceToGameObject(FmodEvents.instance._npcMusicInstance, npcDict[quests[questCount].npcType]);
            FmodEvents.instance.currentNPCPos = npcNewPos;
        }
        else
        {
            Debug.Log("Last available Quest reached");
        }
        npcDict[quests[questCount].npcType].SetActive(true);
        npcNewPos = npcQuestPos[(questCount)].transform.position;
        npcDict[quests[questCount].npcType].transform.position = npcNewPos;
        RuntimeManager.DetachInstanceFromGameObject(FmodEvents.instance._npcMusicInstance);
        RuntimeManager.AttachInstanceToGameObject(FmodEvents.instance._npcMusicInstance, npcDict[quests[questCount].npcType]);
        FmodEvents.instance.currentNPCPos = npcNewPos;
    }
}

public enum QuestType
{
    OnlyDialogue,
    ItemDelivery
}

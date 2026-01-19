using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public int questCount = 0;
    public int questStep = 0;

    public GameObject[] questBoxes;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject[] npcs;
    
    [SerializeField] private Vector2[] npcPos;
    private Dictionary<string, Vector2> npcPosDict = new Dictionary<string, Vector2>();
    
    [SerializeField] private UIManager uIManager;

    void Awake()
    {
        instance = this;
        
        npcPosDict.Add("racoon1", npcPos[0]);
        npcPosDict.Add("racoon2", npcPos[1]);
        npcPosDict.Add("beaver1", npcPos[2]);
        npcPosDict.Add("boar1", npcPos[3]);
        
        EventManager.OnStartQuest.AddListener(StartQuest);
        EventManager.OnAdvanceQuest.AddListener(AdvanceQuest);
        EventManager.OnCompleteQuest.AddListener(CompleteQuest);
        EventManager.OnInteractWithNPC.AddListener(InteractWithNPC);
        EventManager.OnItemsDelivered.AddListener(OnItemsDelivered);
        //EventManager.OnDialogueEnd.AddListener(DialogueEnd);
    }
    void Start()
    {
        EventManager.OnStartQuest.Invoke(questCount);
        questBoxes[0].SetActive(false);
    }

    void InteractWithNPC()
    {
        if (!dialogueManager.dialogueIsShowing)
        {
            if (!dialogueManager.currentDialogueRead)
            {
                //erstes Mal Quest Advancen
                EventManager.OnAdvanceQuest.Invoke(questCount);
                //dialogueManager.StartQuest(questCount);
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
    }

    void AdvanceQuest(int questID)
    {
        questStep++;
        switch (questStep)
        {
            case 2:
                //StartDialog
                EventManager.OnDialogueStart.Invoke();
               return;
            case 3:
                //FinishDialogue, EnableBox
                if (questCount == 0)
                {
                    EventManager.OnCompleteQuest.Invoke(questCount);
                }
                else
                {
                    EnableQuestBox();
                }
                return;
            case 4:
                //ItemsDelivered
                DisableQuestBox();
                return;
            case 5:
                //StartQuestFinishDialogue
                EventManager.OnDialogueStart.Invoke();
                return;
            case 6:
                //FinishDialogue, FinishQuest
                EventManager.OnCompleteQuest.Invoke(questCount);
                return;
        }
    }

    void OnItemsDelivered()
    {
        AdvanceQuest(questCount);
    }
    void CompleteQuest(int questID)
    {
        questStep = 0;
        questCount++;
        EventManager.OnStartQuest.Invoke(questCount);
    }
    void DialogueEnd()
    {
        if (questCount == 0)
        {
            EventManager.OnQuestFinished.Invoke();
            Debug.Log("DialogueEnd played");
        }
    }

    /*public void QuestFinished()
    {
        questCount++;
        if (questCount > 1)
        {
            questBoxes[questCount-2].SetActive(false);
        }
        questBoxes[questCount-1].SetActive(true);
        dialogueManager.StartQuest(questCount);
        switch (questCount)
        {
            case 1:
                npcs[0].transform.position = new Vector3 (npcPosDict["racoon2"].x, npcPosDict["racoon2"].y, 6);
                EventManager.OnFirstQuestComplete.Invoke();
                return;
            case 2:
                npcs[0].SetActive(false);
                npcs[1].transform.position = new Vector3 (npcPosDict["beaver1"].x, npcPosDict["beaver1"].y, 6);
                EventManager.OnSecondQuestComplete.Invoke();
                npcs[1].SetActive(true);
                return;
            case 3:
                npcs[1].SetActive(false);
                npcs[2].transform.position = new Vector3 (npcPosDict["boar1"].x, npcPosDict["boar1"].y, 6);
                npcs[2].SetActive(true);
                EventManager.OnThirdQuestComplete.Invoke();
                return;
        }
    }*/

    void EnableQuestBox()
    {
        questBoxes[questCount-1].SetActive(true);
        uIManager.activeBox = (questCount-1);
    }

    void DisableQuestBox()
    {   
        questBoxes[questCount-1].SetActive(false);
    }

    void UpdateNPCLocations()
    {
        switch (questCount)
        {
            case 0:
                npcs[0].transform.position = new Vector3(npcPosDict["racoon1"].x, npcPosDict["racoon1"].y, 6);
                npcs[1].SetActive(false);
                npcs[2].SetActive(false);
                return;
            case 1:
                npcs[0].transform.position = new Vector3 (npcPosDict["racoon2"].x, npcPosDict["racoon2"].y, 6);
                return;
            case 2:
                npcs[0].SetActive(false);
                npcs[1].transform.position = new Vector3 (npcPosDict["beaver1"].x, npcPosDict["beaver1"].y, 6);
                npcs[1].SetActive(true);
                return;
            case 3:
                npcs[1].SetActive(false);
                npcs[2].transform.position = new Vector3 (npcPosDict["boar1"].x, npcPosDict["boar1"].y, 6);
                npcs[2].SetActive(true);
                return;
        } 
    }
}

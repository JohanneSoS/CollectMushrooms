using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public int questCount = 0;

    public GameObject[] questBoxes;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject[] npcs;
    
    [SerializeField] private Vector2[] npcPos;
    private Dictionary<string, Vector2> npcPosDict = new Dictionary<string, Vector2>();

    void Awake()
    {
        instance = this;
        
        npcPosDict.Add("racoon1", npcPos[0]);
        npcPosDict.Add("racoon2", npcPos[1]);
        npcPosDict.Add("beaver1", npcPos[2]);
        npcPosDict.Add("boar1", npcPos[3]);
        
        EventManager.OnInteractWithNPC.AddListener(InteractWithNPC);
        EventManager.OnQuestFinished.AddListener(QuestFinished);
        EventManager.OnDialogueEnd.AddListener(DialogueEnd);
    }
    void Start()
    {
        questCount = 0;
        npcs[0].transform.position = new Vector3(npcPosDict["racoon1"].x, npcPosDict["racoon1"].y, 6);
        npcs[1].SetActive(false);
        npcs[2].SetActive(false);
    }

    void InteractWithNPC()
    {
        if (!dialogueManager.dialogueIsShowing)
        {
            if (!dialogueManager.currentDialogueRead)
            {
                dialogueManager.StartQuest(questCount);
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

    void DialogueEnd()
    {
        if (questCount == 0)
        {
            EventManager.OnQuestFinished.Invoke();
            Debug.Log("DialogueEnd played");
        }
    }

    public void QuestFinished()
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
    }
}

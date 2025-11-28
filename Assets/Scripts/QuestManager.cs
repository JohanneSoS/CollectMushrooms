using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public int questCount = 0;

    public GameObject[] questBoxes;
    [SerializeField] private DialogueManager dialogueManager;

    void Awake()
    {
        instance = this;
        EventManager.OnQuestFinished.AddListener(AddToQuestCount);
        EventManager.OnInteractWithNPC.AddListener(InteractWithNPC);
        EventManager.OnDialogueEnd.AddListener(EnableNextQuest);
    }
    void Start()
    {
        questCount = 0;
    }

    void AddToQuestCount()
    {
        questCount++;
        switch (questCount)
        {
            case 1: EventManager.OnFirstQuestComplete.Invoke(); break;
            case 2: EventManager.OnSecondQuestComplete.Invoke(); break;
            case 3: EventManager.OnThirdQuestComplete.Invoke(); break;
        }
        questBoxes[questCount-1].SetActive(false);
        questBoxes[questCount].SetActive(true);
    }

    void StartQuest()
    {
        dialogueManager.StartQuest(questCount);
    }

    void ProgressQuest()
    {
        dialogueManager.ProgressQuestUntilFinish(questCount);
    }

    void EnableNextQuest()
    {
        questCount++;
    }
    
    void InteractWithNPC()
    {
        if (!dialogueManager.dialogueIsShowing)
        {
            if (!dialogueManager.currentDialogueRead)
            {
                StartQuest();
            }
            else if (dialogueManager.currentDialogueRead)
            {
                dialogueManager.RepeatLastLine(questCount);
            }
        }
        else if (dialogueManager.dialogueIsShowing)
        {
            ProgressQuest();
        }
        /*        
        switch (npcName)
        {
            case "Racoon":
                if (!dialogueIsShowing)
                {
                    ShowDialogue();
                    JumpToDialogueLine(0);
                }
                else if (dialogueIsShowing && currentLine < (lines.Length-1))
                {
                    ShowNextLine();
                    JumpToDialogueLine(currentLine);
                }
                else if (dialogueIsShowing && currentLine >= (lines.Length-1))
                {
                    HideDialogue();
                }
                return;
        }

        if (!dialogueIsShowing)
        {
            ShowDialogue();
        }*/
    }
}

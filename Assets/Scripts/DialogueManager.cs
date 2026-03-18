using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogTextField;
    public TextMeshProUGUI nameTextField;
    public GameObject dialogBox;
    public bool dialogueIsShowing = false;

    [SerializeField] private QuestManager questManager;
    
    [Header("Dialogue Lines")]
    private DialogueQuest[] questStart;
    private DialogueQuest[] questFinish;

        
    private Dictionary<int, DialogueQuest> dialogueID = new Dictionary<int, DialogueQuest>();
    private Dictionary<int, DialogueQuest> dialogueIDComplete = new Dictionary<int, DialogueQuest>();
    
    [SerializeField] private int currentLine = 0;
    public bool currentDialogueRead = false;
    private int lastActiveQuest = 0;
    public bool itemsDelivered = false;
    private int currentQuestID = 0;
    
    void Awake()
    {
        EventManager.OnStartQuest.AddListener(OnStartQuest);
        EventManager.OnDialogueStart.AddListener(OnDialogueStart);
        EventManager.OnItemsDelivered.AddListener(OnItemsDelivered);
        
        questStart = new DialogueQuest[questManager.quests.Length];
        questFinish = new DialogueQuest[questManager.quests.Length];
        for (int i = 0; i < questManager.quests.Length; i++)
        {
            questStart[i] = questManager.quests[i].startDialogue;
            questFinish[i] = questManager.quests[i].endDialogue;
            dialogueID.Add(i, questStart[i]);
            dialogueIDComplete.Add(i, questFinish[i]);
        }
       
        
        /*dialogueID.Add(0, questStart[0]);
        dialogueID.Add(1, quest1Lines);
        dialogueID.Add(2, quest2Lines);
        dialogueID.Add(3, quest3Lines);
        
        dialogueIDComplete.Add(0, introCompleteLines);
        dialogueIDComplete.Add(1, quest1CompleteLines);
        dialogueIDComplete.Add(2, quest2CompleteLines);
        dialogueIDComplete.Add(3, quest3CompleteLines);*/
    }

    void Start()
    {
        currentLine = 0;
    }

    void Update()
    {
        if (dialogueIsShowing && !dialogBox.activeSelf)
        {
            ShowDialogue();
        }
        else if (!dialogueIsShowing && dialogBox.activeSelf)
        {
            HideDialogue();
        }
    }

    
    public void ShowDialogue()
    {
        dialogBox.SetActive(true);
        dialogueIsShowing = true;
        EventManager.ToggleUI.Invoke(true);
        //disable movement
    }

    public void HideDialogue()
    {
        dialogBox.SetActive(false);
        dialogueIsShowing = false;
        EventManager.ToggleUI.Invoke(false);
        //enable movement
    }

    void OnStartQuest(int questID)
    {
        itemsDelivered = false;
        currentLine = 0;
        currentDialogueRead = false;
        dialogTextField.text = dialogueID[questID].lineTexts[0];
        nameTextField.text = dialogueID[questID].charType.name;
        currentQuestID = questID;
    }

    void OnDialogueStart()
    {
        ShowDialogue();
    }

    public void ProgressQuestUntilFinish(int questID)
    {
        if (itemsDelivered == false)
        {
            if (currentLine < (dialogueID[questID].lineTexts.Length)-1)
            {
                currentLine++;
                dialogTextField.text = dialogueID[questID].lineTexts[currentLine];
                nameTextField.text = dialogueID[questID].charType.name;
            }
            else if (currentLine >= (dialogueID[questID].lineTexts.Length)-1)
            {
                currentDialogueRead = true;
                //EventManager.OnDialogueEnd.Invoke();
                Debug.Log("vor OnAdvanceQuest");
                EventManager.OnAdvanceQuest.Invoke(questID);
                Debug.Log("nach OnAdvanceQuest");
                HideDialogue();
            }
        }
        else
        {
            if (currentLine < (dialogueIDComplete[questID].lineTexts.Length)-1)
            {
                currentLine++;
                dialogTextField.text = dialogueIDComplete[questID].lineTexts[currentLine];
                nameTextField.text = dialogueIDComplete[questID].charType.name;
            }
            else if (currentLine >= (dialogueIDComplete[questID].lineTexts.Length)-1)
            {
                currentDialogueRead = true;
                //EventManager.OnDialogueEnd.Invoke();
                EventManager.OnAdvanceQuest.Invoke(questID);
                HideDialogue();
            }
        }
    }

    public void RepeatLastLine(int questID)
    {
        ShowDialogue();
        dialogTextField.text = dialogueID[questID].lineTexts[(dialogueID[questID].lineTexts.Length)-1];
        nameTextField.text = dialogueID[questID].charType.name;
    }

    void OnItemsDelivered()
    {
        itemsDelivered = true;
        currentLine = 0;
        currentDialogueRead = false;
        dialogTextField.text = dialogueIDComplete[currentQuestID].lineTexts[0];
        nameTextField.text = dialogueIDComplete[currentQuestID].charType.name;
    }
}

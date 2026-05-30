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
    
    //DialogLines
    private DialogueQuest[] questStart;
    private DialogueQuest[] questFinish;

        
    private Dictionary<int, DialogueQuest> dialogueID = new Dictionary<int, DialogueQuest>();
    private Dictionary<int, DialogueQuest> dialogueIDComplete = new Dictionary<int, DialogueQuest>();
    
    [SerializeField] private int currentLine = 0;
    public bool currentDialogueRead = false;
    public bool itemsDelivered = false;
    private int currentQuestID = 0;
    
    void Awake()
    {
        GlobalEventManager.OnStartQuest.AddListener(OnStartQuest);
        GlobalEventManager.OnDialogueStart.AddListener(OnDialogueStart);
        GlobalEventManager.OnItemsDelivered.AddListener(OnItemsDelivered);
        
        questStart = new DialogueQuest[questManager.quests.Length];
        questFinish = new DialogueQuest[questManager.quests.Length];
        for (int i = 0; i < questManager.quests.Length; i++)
        {
            questStart[i] = questManager.quests[i].startDialogue;
            questFinish[i] = questManager.quests[i].endDialogue;
            dialogueID.Add(i, questStart[i]);
            dialogueIDComplete.Add(i, questFinish[i]);
        }
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
        GlobalEventManager.ToggleUI.Invoke(true);
        //disable movement
    }

    public void HideDialogue()
    {
        dialogBox.SetActive(false);
        dialogueIsShowing = false;
        GlobalEventManager.ToggleUI.Invoke(false);
        //enable movement
    }

    void OnStartQuest(int questID)
    {
        if (questID < QuestManager.instance.quests.Length)
        {
            itemsDelivered = false;
            currentLine = 0;
            currentDialogueRead = false;
            dialogTextField.text = dialogueID[questID].lineTexts[0];
            nameTextField.text = dialogueID[questID].charType.name;
            currentQuestID = questID;
        }
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
                Debug.Log("vor OnAdvanceQuest");
                GlobalEventManager.OnAdvanceQuest.Invoke(questID);
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
                GlobalEventManager.OnAdvanceQuest.Invoke(questID);
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

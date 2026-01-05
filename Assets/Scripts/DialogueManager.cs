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
    
    [Header("Dialogue Lines")]
    public DialogueLine[] introLines;
    public DialogueLine[] introCompleteLines;
    public DialogueLine[] quest1Lines;
    public DialogueLine[] quest1CompleteLines;
    public DialogueLine[] quest2Lines;
    public DialogueLine[] quest2CompleteLines;
    public DialogueLine[] quest3Lines;
    public DialogueLine[] quest3CompleteLines;
    
    private Dictionary<int, DialogueLine[]> dialogueID = new Dictionary<int, DialogueLine[]>();
    private Dictionary<int, DialogueLine[]> dialogueIDComplete = new Dictionary<int, DialogueLine[]>();
    
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
        
        dialogueID.Add(0, introLines);
        dialogueID.Add(1, quest1Lines);
        dialogueID.Add(2, quest2Lines);
        dialogueID.Add(3, quest3Lines);
        
        dialogueIDComplete.Add(0, introCompleteLines);
        dialogueIDComplete.Add(1, quest1CompleteLines);
        dialogueIDComplete.Add(2, quest2CompleteLines);
        dialogueIDComplete.Add(3, quest3CompleteLines);
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
        //disable movement
    }

    public void HideDialogue()
    {
        dialogBox.SetActive(false);
        dialogueIsShowing = false;
        //enable movement
    }

    void OnStartQuest(int questID)
    {
        currentLine = 0;
        currentDialogueRead = false;
        dialogTextField.text = dialogueID[questID][0].lineText;
        nameTextField.text = dialogueID[questID][0].charType.name;
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
            if (currentLine < (dialogueID[questID].Length)-1)
            {
                currentLine++;
                dialogTextField.text = dialogueID[questID][currentLine].lineText;
                nameTextField.text = dialogueID[questID][currentLine].charType.name;
            }
            else if (currentLine >= (dialogueID[questID].Length)-1)
            {
                currentDialogueRead = true;
                //EventManager.OnDialogueEnd.Invoke();
                EventManager.OnAdvanceQuest.Invoke(questID);
                HideDialogue();
            }
        }
        else
        {
            if (currentLine < (dialogueIDComplete[questID].Length)-1)
            {
                currentLine++;
                dialogTextField.text = dialogueIDComplete[questID][currentLine].lineText;
                nameTextField.text = dialogueIDComplete[questID][currentLine].charType.name;
            }
            else if (currentLine >= (dialogueIDComplete[questID].Length)-1)
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
        dialogTextField.text = dialogueID[questID][(dialogueID[questID].Length)-1].lineText;
        nameTextField.text = dialogueID[questID][0].charType.name;
    }

    void OnItemsDelivered()
    {
        itemsDelivered = true;
        currentLine = 0;
        currentDialogueRead = false;
        dialogTextField.text = dialogueIDComplete[currentQuestID][0].lineText;
        nameTextField.text = dialogueIDComplete[currentQuestID][0].charType.name;
    }
}

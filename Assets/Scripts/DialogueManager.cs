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
    public DialogueLine[] quest1Lines;
    public DialogueLine[] quest2Lines;
    public DialogueLine[] quest3Lines;
    
    private Dictionary<int, DialogueLine[]> dialogueID = new Dictionary<int, DialogueLine[]>();
    
    [SerializeField] private int currentLine = 0;
    public bool currentDialogueRead = false;
    private int lastActiveQuest = 0;
    
    void Awake()
    {
        dialogueID.Add(0, introLines);
        dialogueID.Add(1, quest1Lines);
        dialogueID.Add(2, quest2Lines);
        dialogueID.Add(3, quest3Lines);
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

    
    void ShowDialogue()
    {
        dialogBox.SetActive(true);
        dialogueIsShowing = true;
    }

    public void HideDialogue()
    {
        dialogBox.SetActive(false);
        dialogueIsShowing = false;
    }

    public void StartQuest(int questID)
    {
        currentLine = 0;
        currentDialogueRead = false;
        dialogTextField.text = dialogueID[questID][0].lineText;
        nameTextField.text = dialogueID[questID][0].charType.name;
        EventManager.OnDialogueStart.Invoke();
        if (questID != 2)
        {
            ShowDialogue(); //Problem: Keine eigene Quest-ID für Dialoge - letzte Quest abgeben und Bieber ansprechen sind die selbe ID - ändern!
        }
    }

    public void ProgressQuestUntilFinish(int questID)
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
            EventManager.OnDialogueEnd.Invoke();
            HideDialogue();
        }
    }

    public void RepeatLastLine(int questID)
    {
        ShowDialogue();
        dialogTextField.text = dialogueID[questID][(dialogueID[questID].Length)-1].lineText;
        nameTextField.text = dialogueID[questID][0].charType.name;
    }


}

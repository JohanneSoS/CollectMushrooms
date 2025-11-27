using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogTextField;
    public GameObject dialogBox;
    [SerializeField] private bool dialogueIsShowing = false;
    
    [Header("Dialogue Lines")]
    public DialogueLine[] lines;
    //public DialogueCharacter[] characters;
    private DialogueCharacter[] characters;
    
    [SerializeField] private int currentLine = 0;
    
    void Awake()
    {
        characters = new DialogueCharacter[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            characters[i] = lines[i].charType;
        }
        EventManager.OnDialogueStart.AddListener(ShowDialogue);
        EventManager.OnDialogueEnd.AddListener(HideDialogue);
        EventManager.OnJumpToDialogueLine.AddListener(JumpToDialogueLine);
        EventManager.OnInteractWithNPC.AddListener(InteractWithNPC);
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

    void HideDialogue()
    {
        dialogBox.SetActive(false);
        dialogueIsShowing = false;
    }

    void ShowNextLine()
    {
        currentLine++;
    }

    void JumpToDialogueLine(int lineID)
    {
        dialogTextField.text = lines[lineID].lineText;
    }

    void InteractWithNPC(string npcName)
    {
        //schreit nach Dictionary
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
        }
    }
}

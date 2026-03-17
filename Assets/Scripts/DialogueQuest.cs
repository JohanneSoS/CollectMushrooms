using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/DialogQuests")]
public class DialogueQuest : ScriptableObject
{
    public DialogueCharacter charType;
    [TextArea]
    public string[] lineTexts;
}

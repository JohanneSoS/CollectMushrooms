using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/DialogLine")]
public class DialogueLine : ScriptableObject
{
    public DialogueCharacter charType;
    [TextArea]
    public string lineText;
}

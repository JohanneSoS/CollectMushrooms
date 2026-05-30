using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Quest")]
public class Quest : ScriptableObject
{
    public DialogueCharacter charType;
    public NPC npcType;
    public DialogueQuest startDialogue;
    public DialogueQuest endDialogue;
    public QuestType questType;

}

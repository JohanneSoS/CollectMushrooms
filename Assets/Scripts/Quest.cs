using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Quest")]
public class Quest : ScriptableObject
{
    public DialogueCharacter charType;
    public NPC npcType;
    public DialogueQuest startDialogue;
    public DialogueQuest endDialogue;
    //public Vector3 charPos = new Vector3(0, 0, 0);
    public GameObject charPosObject;
    public QuestType questType;

    /*void Awake()
    {
        charPos = charPosObject.transform.position;
        //if (questType == QuestType.OnlyDialogue)
        //{
        //    endDialogue = startDialogue;
        //}
    }*/
}

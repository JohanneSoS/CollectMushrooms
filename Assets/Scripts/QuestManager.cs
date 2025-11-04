using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public int questCount;

    public GameObject[] questBoxes;

    void Awake()
    {
        instance = this;
        EventManager.OnQuestFinished.AddListener(AddToQuestCount);
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
        questBoxes[questCount].SetActive(true);
        questBoxes[questCount-1].SetActive(false);
    }
}

using System.Collections;
using NUnit.Framework.Constraints;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject SleepMenu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private GameObject [] QuestMenu;
    //[SerializeField] private GameObject DialogueUI;

    public bool uiActive = false;
    public string currentMenu = "none";

    public int activeBox = 0;

    void Awake()
    {
        EventManager.OpenSleepUI.AddListener(ActivateSleepMenu);
        EventManager.ToggleUI.AddListener(OnUIToggle);
        EventManager.OpenQuestUI.AddListener(OpenQuestUI);
        EventManager.CloseQuestUI.AddListener(ResumeGame);
        EventManager.PauseGame.AddListener(ActivatePauseMenu);
        EventManager.ResumeGame.AddListener(ResumeGame);
        EventManager.ConfirmUI.AddListener(ConfirmUI);
    }
    
    private void OnUIToggle(bool uiState)
    {
        uiActive = uiState;
    }
   /*void Update()
    {
        if (uiActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (currentMenu)
                {
                    case "pause":
                        ResumeGame();
                        return;
                    case "gameover":
                        //ApplyPenalty
                        ResumeGame();
                        return;
                    case "sleep":
                        ResumeGame();
                        return;
                    case "quest":
                        ResumeGame();
                        return;
                }

            }*/

            /*if (Input.GetKeyDown(KeyCode.E) || (Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                switch (currentMenu)
                {
                    case "pause":
                        ResumeGame();
                        return;
                    case "gameover":
                        //ApplyPenalty
                        ResumeGame();
                        return;
                    case "sleep":
                        SkipToDay();
                        return;
                }
            }
        }
    }*/

    public void ConfirmUI()
    {
        switch (currentMenu)
        {
            case "pause":
                ResumeGame();
                break;
            case "gameover":
                //ApplyPenalty
                ResumeGame();
                break;
            case "sleep":
                SkipToDay();
                break;
        }
    }

    void ActivateSleepMenu()
    {
        SleepMenu.SetActive(true);
        currentMenu = "sleep";
        uiActive = true;
        EventManager.ToggleUI.Invoke(true);
    }

    void ActivatePauseMenu()
    {
        PauseMenu.SetActive(true);
        currentMenu = "pause";
        uiActive = true;
        EventManager.ToggleUI.Invoke(true);
    }

    void ActivateGameOverMenu()
    {
        GameOverMenu.SetActive(true);
        currentMenu = "gameover";
        uiActive = true;
        EventManager.ToggleUI.Invoke(true);
    }

    public void ResumeGame()
    {
        SleepMenu.SetActive(false);
        PauseMenu.SetActive(false);
       //GameOverMenu.SetActive(false);
        for (int i = 0; i < QuestMenu.Length; i++)
        {
            QuestMenu[i].SetActive(false);
        }
        uiActive = false;
        currentMenu = "none";
        EventManager.ToggleUI.Invoke(false);
    }

    public void SkipToDay()
    {
        EventManager.OnSkipToDay.Invoke();
        ResumeGame();
    }
    
    void OpenQuestUI()
    {
        StartCoroutine(OpenQuestMenu());
    }

    IEnumerator OpenQuestMenu()
    {
        QuestMenu[(activeBox)].SetActive(true);
        currentMenu = "quest";
        yield return new WaitForSeconds(0.2f);
        uiActive = true;
        EventManager.ToggleUI.Invoke(true);
    }
    
}

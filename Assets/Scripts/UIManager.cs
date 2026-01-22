using System.Collections;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject SleepMenu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private TextMeshProUGUI GameOverText;
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
        EventManager.OnGameOver.AddListener(OnGameOver);
    }
    
    private void OnUIToggle(bool uiState)
    {
        uiActive = uiState;
    }

    public void ConfirmUI()
    {
        switch (currentMenu)
        {
            case "pause":
                ResumeGame();
                break;
            case "gameover":
                EventManager.OnRespawn.Invoke();
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
        GameOverMenu.SetActive(false);
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
        QuestMenu[(activeBox)].SetActive(true);
        currentMenu = "quest";
        //yield return new WaitForSeconds(0.2f);
        uiActive = true;
        EventManager.ToggleUI.Invoke(true);
        //StartCoroutine(OpenQuestMenu());
    }

    /*IEnumerator OpenQuestMenu()
    {
        QuestMenu[(activeBox)].SetActive(true);
        currentMenu = "quest";
        //yield return new WaitForSeconds(0.2f);
        uiActive = true;
        EventManager.ToggleUI.Invoke(true);
    }*/

    void OnGameOver(string deathReason)
    {
        switch (deathReason)
        {
            case "hunger":
                GameOverText.text = "You starved!";
                break;
            case "exhaustion":
                GameOverText.text = "You died of Exhaustion!";
                break;
            case "health":
                GameOverText.text = "You have been slain by a wolf!";
                break;
        }
        GameOverMenu.SetActive(true);
        currentMenu = "gameover";
        uiActive = true;
        EventManager.ToggleUI.Invoke(true);
    }
    
}

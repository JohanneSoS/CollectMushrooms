using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject SleepMenuUI;

    private bool isShowing = false;

    void Awake()
    {
        EventManager.OpenSleepUI.AddListener(OpenSleepUI);
        //EventManager.PauseGame.AddListener(PauseGame);
        //EventManager.ResumeGame.AddListener(ResumeGame);
    }

    void Update()
    {
        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResumeGame();
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            {
                SkipToDay();
            }
        }
    }
    
    public void ResumeGame()
    {
        SleepMenuUI.SetActive(false);
        isShowing = false;
        EventManager.ToggleUI.Invoke(false);
    }

    private void OpenSleepUI()
    {
        SleepMenuUI.SetActive(true);
        isShowing = true;
        EventManager.ToggleUI.Invoke(true);
    }

    public void SkipToDay()
    {
        EventManager.OnSkipToDay.Invoke();
        ResumeGame();
    }
}

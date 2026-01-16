using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;

    private bool isShowing = false;

    void Awake()
    {
        //EventManager.PauseGame.AddListener(PauseGame);
        //EventManager.ResumeGame.AddListener(ResumeGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isShowing)
            {
                EventManager.ResumeGame.Invoke();
                ResumeGame();
            }
            else if (!isShowing)
            {
                EventManager.PauseGame.Invoke();
                PauseGame();
            }
        }
    }
    
    public void ResumeGame()
    {
        MenuUI.SetActive(false);
        isShowing = false;
        EventManager.ToggleUI.Invoke(false);
        EventManager.ResumeGame.Invoke();
    }

    public void PauseGame()
    {
        MenuUI.SetActive(true);
        isShowing = true;
        EventManager.ToggleUI.Invoke(true);
    }
}
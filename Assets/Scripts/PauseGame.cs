using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject canvasRoot;
    public GameObject pauseMenuUI;

    private CanvasGroup pauseMenuGroup;
    private bool isPaused = false;

    void Start()
    {
        // Add or get CanvasGroup component for controlling transparency
        pauseMenuGroup = pauseMenuUI.GetComponent<CanvasGroup>();
        if (pauseMenuGroup == null)
            pauseMenuGroup = pauseMenuUI.AddComponent<CanvasGroup>();

        SetPauseMenuVisible(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                Pause();
        }
    }

    void Pause()
    {
        foreach (Transform child in canvasRoot.transform)
        {
            if (child.gameObject != pauseMenuUI)
                child.gameObject.SetActive(false);
        }

        SetPauseMenuVisible(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        foreach (Transform child in canvasRoot.transform)
        {
            child.gameObject.SetActive(true);
        }

        SetPauseMenuVisible(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void SetPauseMenuVisible(bool visible)
    {
        pauseMenuGroup.alpha = visible ? 1f : 0f;
        pauseMenuGroup.interactable = visible;
        pauseMenuGroup.blocksRaycasts = visible;
    }
}

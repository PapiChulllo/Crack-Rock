using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application; // Explicitly use UnityEngine.Application
using Debug = UnityEngine.Debug; // Explicitly use UnityEngine.Debug

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject achievementsPanel; // Reference to the achievements panel
    public bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void ShowAchievements()
    {
        if (achievementsPanel != null)
        {
            achievementsPanel.SetActive(true);
        }
    }

    public void CloseAchievements()
    {
        if (achievementsPanel != null)
        {
            achievementsPanel.SetActive(false);
        }
    }
}

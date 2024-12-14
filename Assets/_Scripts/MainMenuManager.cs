using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // This function is called when the "Play Game" button is clicked
    public void PlayGame()
    {
        // Load the MainScene
        SceneManager.LoadScene("MainScene");
    }

    // This function is called when the "Quit Game" button is clicked
    public void QuitGame()
    {
        UnityEngine.Debug.Log("Game Quit!");
        UnityEngine.Application.Quit();
    }
}

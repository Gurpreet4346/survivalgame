using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject pauseMenuUI; // Reference to the Pause Menu Canvas
    public GameObject optionsMenuUI; // Reference to the Options Menu Canvas (set later)

    void Update()
    {
        // Toggle pause menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game mechanics
        IsPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game mechanics
        IsPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume game before switching scenes
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenOptionsMenu()
    {
        pauseMenuUI.SetActive(false);
        
            optionsMenuUI.SetActive(true);
        
    }
}

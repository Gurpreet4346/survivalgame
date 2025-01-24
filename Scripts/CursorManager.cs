using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Update()
    {
        if (IsGamePlaying())
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    private bool IsGamePlaying()
    {
        // Check if the game is playing and neither in the main menu nor on the death screen
        return Time.timeScale > 0f && !IsMainMenu() && !IsDeathScreenActive();
    }

    private bool IsMainMenu()
    {
        // Example condition: Replace with your actual main menu detection logic
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu";
    }

    private bool IsDeathScreenActive()
    {
        // Example condition: Replace with your actual death screen detection logic
        var deathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
        return deathScreen != null && deathScreen.activeSelf;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

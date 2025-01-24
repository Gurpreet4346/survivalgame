using UnityEngine;

public class DisableCameraInMainMenu : MonoBehaviour
{
    void Start()
    {
        // Disable this camera in the Main Menu
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
        else
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
        }
    }
}

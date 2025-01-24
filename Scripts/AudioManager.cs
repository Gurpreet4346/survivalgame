using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource mainMenuMusic;
    public AudioSource gameMusic;

    [Header("Sound Effects")]
    public AudioSource tileTouchSFX;
    public AudioSource playerDamageSFX;
    public AudioSource staminaDepletedSFX;
    public AudioSource scarecrowDamageSFX;
    public AudioSource scarecrowFireSFX;
    public AudioSource golemPrepareSFX;
    public AudioSource golemChargeSFX;
    public AudioSource uiButtonClickSFX;
    public AudioSource playerDashSFX;

    [Header("Audio Settings")]
    public bool isMusicOn = true;
    public bool isSFXOn = true;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioSource music)
    {
        if (isMusicOn)
        {
            StopAllMusic();
            music.Play();
        }
    }

    public void StopAllMusic()
    {
        mainMenuMusic.Stop();
        gameMusic.Stop();
    }

    public void PlaySFX(AudioSource sfx)
    {
        if (isSFXOn && sfx != null)
        {
            sfx.Play();
        }
    }

    public void ToggleMusic(bool state)
    {
        isMusicOn = state;
        if (!state)
        {
            StopAllMusic();
        }
        else
        {
            // Resume music based on current scene
            if (mainMenuMusic.isPlaying == false && gameMusic.isPlaying == false)
            {
                mainMenuMusic.Play(); // Default fallback
            }
        }
    }

    public void ToggleSFX(bool state)
    {
        isSFXOn = state;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DynamicToggleAssigner : MonoBehaviour
{
    public Toggle musicToggle; // Reference to the Toggle in the Options Menu

    private void Start()
    {
        // Ensure the toggle is assigned
        if (musicToggle == null)
        {
            Debug.LogError("Music Toggle is not assigned in the Inspector.");
            return;
        }

        // Try to find the AudioManager
        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            // Dynamically assign the ToggleMusic method to the toggle
            musicToggle.onValueChanged.AddListener(audioManager.ToggleMusic);

            // Set the toggle state based on the AudioManager's current setting
            musicToggle.isOn = audioManager.isMusicOn;
        }
        else
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }
}

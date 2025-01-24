using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using System.Collections;

public class DesertTrigger : MonoBehaviour
{
    private bool hasTriggered = false;
    public string message; // The message to display (e.g., "Plains," "Desert")
    public float fadeDuration = 1f; // Time it takes for the message to fade in/out
    public float displayDuration = 2f; // How long the message stays fully visible
    public Material newSkybox; // The new Skybox material to switch to

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            ShowMessage(message);
            ChangeSkybox();

            var playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.RegenerateHealth();
                Debug.Log("Player's health fully restored!");
            }
        }
    }

    private void ShowMessage(string message)
    {
        var uiManager = Object.FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.DisplayMessage(message, fadeDuration, displayDuration);
        }
    }

    private void ChangeSkybox()
    {
        if (newSkybox != null)
        {
            RenderSettings.skybox = newSkybox;
            Debug.Log($"Skybox changed to: {newSkybox.name}");
        }
        else
        {
            Debug.LogWarning("No Skybox material assigned to the trigger.");
        }
    }
}

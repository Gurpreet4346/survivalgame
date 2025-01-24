using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using System.Collections;

public class ScoreTrigger : MonoBehaviour
{
    private bool hasTriggered = false;
    public string message; // The message to display (e.g., "Plains," "Desert")
    public float fadeDuration = 1f; // Time it takes for the message to fade in/out
    public float displayDuration = 2f; // How long the message stays fully visible

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // Start scoring logic
            var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.StartScoring();
            }

            // Show the message
            ShowMessage(message);
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
}

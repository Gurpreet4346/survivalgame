using UnityEngine;
using TMPro; // For TextMeshPro components
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Assign this to the TextMeshProUGUI element in the Inspector

    public void DisplayMessage(string message, float fadeDuration, float displayDuration)
    {
        StartCoroutine(FadeMessageCoroutine(message, fadeDuration, displayDuration));
    }

    private IEnumerator FadeMessageCoroutine(string message, float fadeDuration, float displayDuration)
    {
        // Set the message and reset transparency
        messageText.text = message;
        Color color = messageText.color;
        color.a = 0f;
        messageText.color = color;

        // Fade in
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            messageText.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure fully visible
        color.a = 1f;
        messageText.color = color;

        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            color.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            messageText.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure fully transparent
        color.a = 0f;
        messageText.color = color;
    }
}

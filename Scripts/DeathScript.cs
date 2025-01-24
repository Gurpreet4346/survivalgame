using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Use Object.FindFirstObjectByType to get the ScoreManager
            var scoreManager = Object.FindFirstObjectByType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.StopScoring();
            }

            // Optionally, add player death mechanics here
            Debug.Log("Player has died!");
        }
    }
}

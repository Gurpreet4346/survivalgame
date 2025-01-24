using UnityEngine;
using UnityEngine.UI;
using TMPro; // If using TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance

    public TextMeshProUGUI scoreText; // Reference to the Score UI Text
    public GameObject deathScreenUI; // Reference to the Death Screen UI
    public TextMeshProUGUI finalScoreText; // Text to show the final score
    public TextMeshProUGUI highScoreText; // Text to show the high score

    private float currentScore = 0f;
    private bool isScoring = false;
    private float highScore = 0f;

    private void Awake()
    {
        // Singleton implementation
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load the high score, default to 0 if not set
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        UpdateScoreUI();
    }

    void Update()
    {
        if (isScoring)
        {
            currentScore += Time.deltaTime;
            UpdateScoreUI();
        }
    }

    public void AddScore(float points)
    {
        currentScore += points;
        UpdateScoreUI();
    }

    public void StartScoring()
    {
        isScoring = true;
    }

    public void StopScoring()
    {
        isScoring = false;
        UpdateHighScore();
        ShowDeathScreen();
    }

    private void UpdateHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save(); // Save immediately
        }
    }

    private void UpdateScoreUI()
    {
        // Format score to 4 digits
        scoreText.text = currentScore.ToString("0000");
    }

    private void ShowDeathScreen()
    {
        // Show the death screen
        deathScreenUI.SetActive(true);

        // Update the final score and high score
        finalScoreText.text = "Your Score: " + currentScore.ToString("0000");
        highScoreText.text = "High Score: " + highScore.ToString("0000");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Resume game before switching scenes
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}

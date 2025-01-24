using UnityEngine;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public Transform characterPreviewSpot; // Where characters are displayed
    public TextMeshProUGUI nameText, healthText, speedText,weaponText; // UI elements for stats
    public GameObject[] characterPrefabs; // Array of character prefabs
    private GameObject currentCharacter;  // Currently displayed character
    private int currentIndex = 0;         // Index of the selected character
   


    private void Start()
    {
        ShowCharacter(0); // Show the first character
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % characterPrefabs.Length;
        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        // Ensure the index wraps correctly
        currentIndex = (currentIndex - 1 + characterPrefabs.Length) % characterPrefabs.Length;
        ShowCharacter(currentIndex);
    }


    private void ShowCharacter(int index)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        // Instantiate the selected character prefab
        currentCharacter = Instantiate(characterPrefabs[index], characterPreviewSpot.position, Quaternion.identity);
        currentCharacter.transform.SetParent(characterPreviewSpot);

        // Scale the character for the Main Menu
        currentCharacter.transform.localScale = Vector3.one * 2f; // Adjust as needed

        // Ensure the character faces the correct direction
        currentCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);

        // Update stats in the UI
        CharacterInfo info = currentCharacter.GetComponent<CharacterInfo>();
        if (info != null && info.stats != null)
        {
            nameText.text = $"Name: {info.stats.characterName}";
            healthText.text = $"Health: {info.stats.health}";
            speedText.text = $"Speed: {info.stats.speed}";
            weaponText.text = $"Weapon: {info.stats.weapon}";
        }
    }



    public void PlayGame()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex); // Save selected character index
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); // Load the gameplay scene
    }
}

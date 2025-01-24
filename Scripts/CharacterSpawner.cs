using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public Transform spawnPoint;          // Spawn location
    public GameObject[] characterPrefabs; // Array of character prefabs

    void Start()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject character = Instantiate(characterPrefabs[selectedCharacterIndex], spawnPoint.position, Quaternion.identity);

        // Apply stats
        CharacterInfo info = character.GetComponent<CharacterInfo>();
        if (info != null && info.stats != null)
        {
            PlayerController controller = character.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.SetStats(info.stats.health, info.stats.speed);
            }
        }
    }
}

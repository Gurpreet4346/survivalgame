using UnityEngine;
using System.Collections;

public class GolemSpawner : MonoBehaviour
{
    public GameObject golemPrefab; // The Golem prefab
    public float spawnYLevel = -20f; // Y-level where the Golem spawns
    public float spawnDelay = 3f; // Delay before spawning the Golem
    public float despawnYOffset = -10f; // Y-level offset for despawning the Golem
    public int maxGolems = 1; // Maximum number of Golems at a time

    private GameObject activeGolem; // Reference to the active Golem
    private Transform player; // Reference to the player
    private bool playerInRange = false; // Whether the player is in range of this floor
    private bool isSpawning = false; // Whether a Golem is currently being spawned

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private void Update()
    {
        if (player == null) return;

        // Check if the player is on this floor
        playerInRange = Mathf.Abs(player.position.y - spawnYLevel) <= 5f;

        if (playerInRange)
        {
            // If there is no active Golem and we're not already spawning one, spawn a new Golem
            if (activeGolem == null && !isSpawning)
            {
                StartCoroutine(SpawnGolemWithDelay());
            }
        }
        else
        {
            // Despawn the active Golem if the player leaves the floor
            if (activeGolem != null)
            {
                DespawnGolem();
            }
        }

        // Despawn the Golem if it falls below the despawn threshold
        if (activeGolem != null && activeGolem.transform.position.y < player.position.y + despawnYOffset)
        {
            DespawnGolem();
        }
    }

    private IEnumerator FindPlayer()
    {
        while (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                Debug.Log($"Player found: {player.name}");
            }
            yield return new WaitForSeconds(0.5f); // Retry every 0.5 seconds
        }
    }

    private IEnumerator SpawnGolemWithDelay()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnDelay);

        if (activeGolem == null && playerInRange)
        {
            // Find all tiles at the spawnYLevel
            HexTile[] tiles = FindObjectsOfType<HexTile>();
            foreach (HexTile tile in tiles)
            {
                if (Mathf.Abs(tile.transform.position.y - spawnYLevel) < 0.1f) // Ensure the tile is on the same level
                {
                    Vector3 spawnPosition = tile.transform.position + Vector3.up * 1f; // Offset to prevent clipping
                    activeGolem = Instantiate(golemPrefab, spawnPosition, Quaternion.identity);
                    Debug.Log($"Golem spawned at {spawnPosition}");
                    break;
                }
            }
        }

        isSpawning = false;
    }

    private void DespawnGolem()
    {
        if (activeGolem != null)
        {
            Debug.Log($"Golem despawned at {activeGolem.transform.position}");
            Destroy(activeGolem);
            activeGolem = null;
        }
    }
}

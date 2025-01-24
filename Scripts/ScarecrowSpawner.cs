using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScarecrowSpawner : MonoBehaviour
{
    public GameObject scarecrowPrefab;
    public float spawnYLevel = 0f; // Fixed Y-level for spawning
    public float spawnRange = 5f; // Range around the Y-level for spawning
    public int maxScarecrows = 3; // Maximum scarecrows at a time
    public float respawnDelay = 3f; // Time delay for respawning scarecrows
    public float minSpawnDistance = 3f; // Minimum distance between scarecrows
    public float minDistanceFromPlayer = 3f; // Minimum distance from the player for spawning

    private Transform player; // Reference to the player
    private List<GameObject> activeScarecrows = new List<GameObject>();
    private bool playerInRange = false;

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private void Update()
    {
        if (player == null) return; // Skip logic if player is not assigned

        // Check if the player is within the range of the Y-level
        playerInRange = Mathf.Abs(player.position.y - spawnYLevel) <= spawnRange;

        if (playerInRange)
        {
            ManageSpawning();
        }
        else
        {
            DespawnAllScarecrows();
        }
    }

    IEnumerator FindPlayer()
    {
        // Keep checking until the player is found
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

    void ManageSpawning()
    {
        // Remove any destroyed scarecrows from the list
        activeScarecrows.RemoveAll(scarecrow => scarecrow == null);

        // Spawn scarecrows if below the maximum
        if (activeScarecrows.Count < maxScarecrows)
        {
            StartCoroutine(SpawnScarecrow());
        }
    }

    IEnumerator SpawnScarecrow()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (activeScarecrows.Count < maxScarecrows)
        {
            HexTile[] tiles = FindObjectsOfType<HexTile>();
            foreach (HexTile tile in tiles)
            {
                // Filter tiles to only include those on the correct floor
                if (Mathf.Abs(tile.transform.position.y - spawnYLevel) > 0.1f) // Allow a small margin for floating-point errors
                {
                    continue;
                }

                Vector3 spawnPosition = tile.transform.position + Vector3.up * 1f;

                // Ensure the spawn position is valid
                if (IsPositionValid(spawnPosition))
                {
                    Debug.Log($"Spawning scarecrow at {spawnPosition}");

                    GameObject scarecrow = Instantiate(scarecrowPrefab, spawnPosition, Quaternion.identity);
                    activeScarecrows.Add(scarecrow);
                    break;
                }
            }
        }
    }

    void DespawnAllScarecrows()
    {
        foreach (GameObject scarecrow in activeScarecrows)
        {
            if (scarecrow != null)
            {
                Destroy(scarecrow);
            }
        }

        activeScarecrows.Clear();
    }

    bool IsPositionValid(Vector3 position)
    {
        // Check distance from existing scarecrows
        foreach (GameObject scarecrow in activeScarecrows)
        {
            if (scarecrow != null && Vector3.Distance(scarecrow.transform.position, position) < minSpawnDistance)
            {
                return false;
            }
        }

        // Check distance from the player
        if (player != null && Vector3.Distance(player.position, position) < minDistanceFromPlayer)
        {
            return false;
        }

        return true;
    }

    bool TileHasScarecrow(Vector3 tilePosition)
    {
        foreach (GameObject scarecrow in activeScarecrows)
        {
            if (Vector3.Distance(scarecrow.transform.position, tilePosition) < 0.1f)
            {
                return true;
            }
        }

        return false;
    }
}

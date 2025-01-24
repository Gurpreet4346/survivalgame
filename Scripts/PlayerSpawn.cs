using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Player prefab
    public Transform spawnPoint;   // Fallback spawn point

    void Start()
    {
        GameObject firstTile = GameObject.FindGameObjectWithTag("HexTile");
        Vector3 spawnPosition = firstTile ? firstTile.transform.position : spawnPoint.position;
        Instantiate(playerPrefab, spawnPosition + Vector3.up * 1.5f, Quaternion.identity);
    }
}

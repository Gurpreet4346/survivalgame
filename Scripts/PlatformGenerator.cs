using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;      // The prefab for a single hexagonal tile
    public int platformWidth = 5;        // Number of tiles in one row
    public int platformHeight = 5;       // Number of rows
    public float tileSize = 1.5f;        // Size of the hexagonal tile
    public int numberOfFloors = 3;       // Number of floors
    public float floorHeight = 3f;       // Height difference between floors

    public void GeneratePlatforms()
    {
        // Create a parent GameObject to hold all platforms
        GameObject platformsParent = new GameObject("GeneratedPlatforms");

        for (int floor = 0; floor < numberOfFloors; floor++)
        {
            // Create a new parent GameObject for each floor
            GameObject floorParent = new GameObject($"Floor_{floor}");
            floorParent.transform.parent = platformsParent.transform;

            Vector3 floorOffset = new Vector3(0, -floor * floorHeight, 0);

            for (int row = 0; row < platformHeight; row++)
            {
                for (int col = 0; col < platformWidth; col++)
                {
                    // Offset for hexagonal placement
                    float xOffset = (row % 2 == 0) ? 0 : tileSize * 0.5f;
                    Vector3 position = new Vector3(col * tileSize + xOffset, 0, row * tileSize * 0.866f) + floorOffset;

                    // Instantiate tile and set its parent to the floor parent
                    GameObject tile = Instantiate(hexTilePrefab, position, Quaternion.identity);
                    tile.transform.parent = floorParent.transform;
                }
            }
        }
    }
}

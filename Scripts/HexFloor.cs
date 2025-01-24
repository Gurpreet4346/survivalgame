using UnityEngine;

public class HexagonalFloorGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;  // The prefab for a single hexagonal tile
    public float floorSize = 10f;     // Horizontal stretch size of the floor
    public int radius = 3;            // Radius of the hexagonal grid
    public float tileSpacing = 0.1f;  // Spacing between hex tiles

    public void GenerateFloor()
    {
        // Clear existing tiles in the scene
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // Calculate the effective tile size including spacing
        float tileWidth = hexTilePrefab.transform.localScale.x + tileSpacing;
        float tileHeight = Mathf.Sqrt(3) * tileWidth * 0.5f;

        // Scale tiles horizontally to match floor size
        float horizontalScale = floorSize / (2 * radius * tileWidth);
        hexTilePrefab.transform.localScale = new Vector3(horizontalScale, hexTilePrefab.transform.localScale.y, horizontalScale);

        // Generate hexagonal grid using axial coordinates (q, r)
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);

            for (int r = r1; r <= r2; r++)
            {
                Vector3 position = HexToWorldPosition(q, r, tileWidth, tileHeight);
                GameObject tile = Instantiate(hexTilePrefab, position, Quaternion.identity);

                // Assign parent after instantiation
                tile.transform.SetParent(transform, false);
            }
        }
    }

    Vector3 HexToWorldPosition(int q, int r, float tileWidth, float tileHeight)
    {
        float x = tileWidth * (q + r * 0.5f);
        float z = tileHeight * r;
        return new Vector3(x, 0, z);
    }
}

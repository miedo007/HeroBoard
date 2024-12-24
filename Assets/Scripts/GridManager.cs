using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;  // Number of columns
    public int height = 5; // Number of rows
    public GameObject tilePrefab; // Reference to the tile prefab

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++) // Loop through columns
        {
            for (int y = 0; y < height; y++) // Loop through rows
            {
                // Calculate the position of the tile
                Vector3 position = new Vector3(x * 1.02f, -y * 1.02f, 0); // Adjust spacing
                // Instantiate the tile prefab at the calculated position
                Instantiate(tilePrefab, position, Quaternion.identity);
            }
        }
    }
}

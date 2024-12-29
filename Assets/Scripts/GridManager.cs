using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int columns = 5;
    public int rows = 5;
    public float tileSpacing = 0.1f; // Add this for spacing between tiles
    public GameObject tilePrefab;

    private CameraSetup cameraSetup;

    void Start()
    {
        cameraSetup = FindObjectOfType<CameraSetup>();
        if (cameraSetup == null)
        {
            Debug.LogError("CameraSetup not found!");
            return;
        }

        GenerateGrid();
        cameraSetup.SetBoardSize(columns, rows); // Update camera based on grid size
    }

    void GenerateGrid()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Adjust position to include spacing
                Vector3 position = new Vector3(x + x * 0.02f, y + y * 0.02f, 0);
                Instantiate(tilePrefab, position, Quaternion.identity);
            }
        }
    }
}

using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab; // Tile prefab to create tiles
    public int gridWidth = 5; // Number of columns
    public int gridHeight = 5; // Number of rows
    public Camera mainCamera; // Reference to the main camera

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; // Use the main camera if none assigned

        GenerateGrid();
        CenterCameraOnGrid();
    }

   void GenerateGrid()
{
    float tileSpacing = 0.1f; // Adjust this value to control the spacing between tiles

    for (int x = 0; x < gridWidth; x++)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            // Apply spacing to the position
            Vector3 tilePosition = new Vector3(x + x * 0.02f, y + y * 0.02f, 0);
            Instantiate(tilePrefab, tilePosition, Quaternion.identity);
        }
    }
}


    void CenterCameraOnGrid()
    {
        // Calculate the center of the grid
        Vector3 gridCenter = new Vector3((gridWidth - 1) / 2f, (gridHeight - 1) / 2f, -10);

        // Set the camera position
        mainCamera.transform.position = gridCenter;

        // Adjust the camera's orthographic size to fit the grid
        float aspectRatio = (float)Screen.width / Screen.height;
        float verticalSize = gridHeight / 2f;
        float horizontalSize = gridWidth / (2f * aspectRatio);

        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
}

using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public Camera mainCamera;
    public Transform boardTransform; // Reference to the board or parent object
    public Vector2 boardSize; // Size of the board in units (Width, Height)

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Center the camera on the board
        Vector3 boardCenter = new Vector3(boardSize.x / 2, boardSize.y / 2, -10);
        mainCamera.transform.position = boardCenter;

        // Adjust the orthographic size to fit the board
        float aspectRatio = (float)Screen.width / Screen.height;
        float requiredSize = boardSize.y / 2;

        if (boardSize.x / aspectRatio > requiredSize)
        {
            requiredSize = boardSize.x / (2 * aspectRatio);
        }

        mainCamera.orthographicSize = requiredSize;
    }
}

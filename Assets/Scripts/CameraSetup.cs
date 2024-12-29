using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public Camera mainCamera;
    public float verticalExtraSpace = 1.5f; // Additional vertical space for health bars

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void SetBoardSize(int width, int height)
    {
        // Center the camera
        Vector3 boardCenter = new Vector3((width - 1) / 2f, (height - 1) / 2f, -10);
        mainCamera.transform.position = boardCenter;

        // Adjust orthographic size to include extra space
        float aspectRatio = (float)Screen.width / Screen.height;
        float verticalSize = (height / 2f) + verticalExtraSpace;
        float horizontalSize = (width / 2f) / aspectRatio;

        // Use the larger of the two to ensure grid fits
        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);

        Debug.Log($"Camera adjusted for board size: {width}x{height}");
    }
}

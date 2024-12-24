using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public Tile SelectedTile; // Currently selected tile

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign the singleton instance
        }
        else
        {
            Destroy(gameObject); // Ensure only one GameManager exists
        }
    }

    public void SelectTile(Tile tile)
    {
        if (SelectedTile != null)
        {
            SelectedTile.Deselect(); // Deselect the previously selected tile
        }

        SelectedTile = tile; // Assign the new selected tile
        Debug.Log("GameManager: Selected tile is now " + (tile != null ? tile.name : "null"));
    }
}

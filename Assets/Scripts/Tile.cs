using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    public GameObject unitOnTile; // The unit currently on this tile
    public bool isSelected; // Whether this tile is currently selected
    public int movementRange = 1; // Movement range for units

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color; // Store the original tile color

        // Optional: Randomly spawn units for testing
        if (Random.value > 0.7f) // 30% chance to spawn a unit
        {
            SpawnUnit();
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Tile clicked!");

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            return;
        }

        // Case 1: Deselect the currently selected tile
        if (isSelected)
        {
            Deselect();
            GameManager.Instance.SelectTile(null);
            return;
        }

        // Case 2: Select a tile with a unit
        if (unitOnTile != null)
        {
            Tile selectedTile = GameManager.Instance.SelectedTile;

            // If a unit is already selected, check if the clicked unit is an enemy
            if (selectedTile != null && selectedTile.unitOnTile != null)
            {
                UnitBase selectedUnit = selectedTile.unitOnTile.GetComponent<UnitBase>();
                UnitBase targetUnit = unitOnTile.GetComponent<UnitBase>();

                if (selectedUnit != null && targetUnit != null)
                {
                    // Check if the target unit is an enemy (different team)
                    if (selectedUnit.teamID != targetUnit.teamID && IsWithinRange(selectedTile))
                    {
                        Debug.Log("Attacking enemy unit on tile: " + name);
                        selectedUnit.Attack(targetUnit); // Perform the attack
                        HighlightValidAttackTargets(false);
                        HighlightValidTiles(false);
                        return;
                    }
                    else
                    {
                        Debug.Log("Switching selection to friendly unit on tile: " + name);
                    }
                }
            }

            // If no valid attack, switch selection to this unit
            GameManager.Instance.SelectTile(this);
            isSelected = true;
            _spriteRenderer.color = Color.yellow; // Highlight the selected tile
            HighlightValidTiles(true); // Highlight valid movement tiles
            HighlightValidAttackTargets(true); // Highlight valid attack targets
            return;
        }

        // Case 3: Move the selected unit to an empty tile
        if (GameManager.Instance.SelectedTile != null && unitOnTile == null)
        {
            Tile selectedTile = GameManager.Instance.SelectedTile;
            if (IsWithinRange(selectedTile))
            {
                MoveUnitHere(selectedTile);
                HighlightValidTiles(false);
                HighlightValidAttackTargets(false);
            }
            else
            {
                Debug.Log("Tile is out of range.");
            }
            return;
        }

        Debug.Log("No valid action for this tile.");
    }

    public void Deselect()
    {
        _spriteRenderer.color = _originalColor; // Reset the tile color
        isSelected = false;
        HighlightValidTiles(false); // Remove movement highlights
        HighlightValidAttackTargets(false); // Remove attack highlights
    }

    void SpawnUnit()
    {
        if (unitOnTile == null)
        {
            // Instantiate a unit prefab at the tile's position
            GameObject unit = Instantiate(Resources.Load<GameObject>("Unit"), transform.position, Quaternion.identity);
            unitOnTile = unit; // Assign the unit to this tile

            // Assign a random teamID for testing purposes
            UnitBase unitBase = unit.GetComponent<UnitBase>();
            if (unitBase != null)
            {
                unitBase.teamID = Random.Range(0, 2); // 0 for Team 1, 1 for Team 2
                Debug.Log("Unit spawned on tile: " + name + " | Team: " + unitBase.teamID);

                // Optional: Change the unit's color based on team
                SpriteRenderer renderer = unit.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = (unitBase.teamID == 0) ? Color.blue : Color.red; // Blue for Team 1, Red for Team 2
                }
            }
        }
        else
        {
            Debug.Log("A unit is already on this tile!");
        }
    }

    void MoveUnitHere(Tile fromTile)
    {
        if (fromTile == null || fromTile.unitOnTile == null)
        {
            Debug.LogError("No unit to move!");
            return;
        }

        Debug.Log("Moving unit from " + fromTile.name + " to " + name);

        // Move the unit to this tile
        unitOnTile = fromTile.unitOnTile;
        unitOnTile.transform.position = transform.position; // Update the unit's position

        // Clear the old tile's unit reference
        fromTile.unitOnTile = null;

        // Deselect the old tile
        fromTile.Deselect();

        // Clear selection in GameManager
        GameManager.Instance.SelectTile(null);
    }

    void HighlightValidTiles(bool highlight)
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile != this && tile.unitOnTile == null && IsWithinRange(tile))
            {
                tile._spriteRenderer.color = highlight ? Color.green : tile._originalColor;
            }
        }
    }

    void HighlightValidAttackTargets(bool highlight)
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.unitOnTile != null && tile.unitOnTile != unitOnTile && IsWithinRange(tile))
            {
                tile._spriteRenderer.color = highlight ? Color.red : tile._originalColor;
            }
        }
    }

    bool IsWithinRange(Tile targetTile)
    {
        // Calculate the Manhattan distance
        int distance = Mathf.Abs((int)(targetTile.transform.position.x - transform.position.x)) +
                       Mathf.Abs((int)(targetTile.transform.position.y - transform.position.y));
        return distance <= movementRange;
    }
}

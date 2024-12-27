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

        // Randomly spawn units for testing
        if (Random.value > 0.7f) // 30% chance to spawn a unit
        {
            SpawnUnit();
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Tile clicked!");

        if (TurnManager.Instance == null)
        {
            Debug.LogError("TurnManager.Instance is null!");
            return;
        }

        // Case 1: Deselect the currently selected tile
        if (isSelected)
        {
            Deselect();
            GameManager.Instance.SelectTile(null);
            FindObjectOfType<UnitDetailsPanel>()?.ClearDetails(); // Clear details if no unit
            return;
        }

        // Case 2: If a tile with a unit is clicked
        if (unitOnTile != null)
        {
            UnitBase targetUnit = unitOnTile.GetComponent<UnitBase>();

            // Ensure the clicked unit belongs to the active player
            if (!TurnManager.Instance.IsCurrentPlayer(targetUnit.teamID))
            {
                Tile selectedTile = GameManager.Instance.SelectedTile;

                // Check if an enemy unit is targeted and the selected unit can attack
                if (selectedTile != null && selectedTile.unitOnTile != null)
                {
                    UnitBase selectedUnit = selectedTile.unitOnTile.GetComponent<UnitBase>();
                    UnitTurnState turnState = selectedTile.unitOnTile.GetComponent<UnitTurnState>();

                    // Check if the selected unit can attack
                    if (selectedUnit != null && turnState != null && !turnState.hasAttacked && selectedUnit.teamID != targetUnit.teamID)
                    {
                        if (IsWithinRange(selectedTile))
                        {
                            Debug.Log($"Attacking enemy unit on tile: {name}");
                            selectedUnit.Attack(targetUnit); // Perform the attack
                            turnState.hasAttacked = true; // Mark the unit as having attacked
                            TurnManager.Instance.RegisterAttack(); // Register the attack globally
                            HighlightValidAttackTargets(false);
                            HighlightValidTiles(false);
                            GameManager.Instance.SelectTile(null); // Deselect after the attack
                            FindObjectOfType<UnitDetailsPanel>()?.ClearDetails(); // Clear details after attack
                            return;
                        }
                        else
                        {
                            Debug.Log("Enemy unit is out of attack range.");
                            return;
                        }
                    }
                }

                Debug.Log("It's not this unit's turn!");
                return;
            }

            // If a friendly unit is clicked, select it
            GameManager.Instance.SelectTile(this);
            isSelected = true;
            _spriteRenderer.color = Color.yellow; // Highlight the selected tile
            HighlightValidTiles(true); // Highlight valid movement tiles
            HighlightValidAttackTargets(true); // Highlight valid attack targets

            // Update the Unit Details Panel
            if (GameManager.Instance != null)
            {
                UnitDetailsPanel detailsPanel = FindObjectOfType<UnitDetailsPanel>();
                UnitBase unit = unitOnTile.GetComponent<UnitBase>();
                UnitTurnState turnState = unitOnTile.GetComponent<UnitTurnState>();

                Debug.Log($"Unit: {unit}, TurnState: {turnState}");
                detailsPanel?.UpdateUnitDetails(unit, turnState); // Update unit details
            }
            return;
        }

        // Case 3: Move the selected unit to an empty tile
        if (GameManager.Instance.SelectedTile != null && unitOnTile == null)
        {
            Tile selectedTile = GameManager.Instance.SelectedTile;
            UnitBase selectedUnit = selectedTile.unitOnTile?.GetComponent<UnitBase>();
            UnitTurnState turnState = selectedTile.unitOnTile?.GetComponent<UnitTurnState>();

            if (IsWithinRange(selectedTile) && selectedUnit != null && turnState != null && !turnState.hasMoved)
            {
                MoveUnitHere(selectedTile);
                turnState.hasMoved = true; // Mark the unit as having moved
                TurnManager.Instance.RegisterMove(); // Register the move globally
                HighlightValidTiles(false);

                // Update the Unit Details Panel
                UnitDetailsPanel detailsPanel = FindObjectOfType<UnitDetailsPanel>();
                detailsPanel?.UpdateUnitDetails(selectedUnit, turnState); // Update details after move
            }
            else
            {
                Debug.Log("Tile is out of range or unit has already moved.");
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

            // Add the UnitTurnState component for tracking actions
            unit.AddComponent<UnitTurnState>();
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
        unitOnTile.transform.position = transform.position;

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

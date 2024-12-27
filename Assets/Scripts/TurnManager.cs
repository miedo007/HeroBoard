using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance; // Singleton instance

    public int currentPlayer = 0; // 0 = Player 1, 1 = Player 2
    private bool hasMoved = false; // Tracks if a move has been made this turn
    private bool hasAttacked = false; // Tracks if an attack has been made this turn

    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        Debug.Log("TurnManager Instance initialized.");
    }
    else
    {
        Destroy(gameObject); // Ensure only one TurnManager exists
        Debug.LogError("Duplicate TurnManager detected and destroyed.");
    }
}


    public bool IsCurrentPlayer(int teamID)
    {
        return teamID == currentPlayer;
    }

    public void RegisterMove()
    {
        hasMoved = true;
        CheckTurnEnd();
    }

    public void RegisterAttack()
    {
        hasAttacked = true;
        CheckTurnEnd();
    }

    private void CheckTurnEnd()
    {
        if (hasMoved && hasAttacked)
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        currentPlayer = (currentPlayer + 1) % 2; // Alternate between players
        hasMoved = false;
        hasAttacked = false;

        // Reset all units' states and update highlights
        foreach (UnitTurnState unitState in FindObjectsOfType<UnitTurnState>())
        {
            bool isCurrentPlayer = IsCurrentPlayer(unitState.GetComponent<UnitBase>().teamID);
            if (isCurrentPlayer)
            {
                unitState.ResetState(); // Reset state for current player's units
            }
            unitState.UpdateHighlight(); // Update highlight for all units
        }

        // Update turn indicator
        FindObjectOfType<TurnIndicatorManager>()?.UpdateTurnIndicator();

        Debug.Log($"Player {currentPlayer + 1}'s Turn!");
    }
}

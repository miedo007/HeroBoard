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
            Instance = this; // Assign the singleton instance
        }
        else
        {
            Destroy(gameObject); // Ensure only one TurnManager exists
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

        Debug.Log($"Player {currentPlayer + 1}'s Turn!");
    }
}

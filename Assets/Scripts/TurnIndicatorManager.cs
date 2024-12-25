using TMPro;
using UnityEngine;

public class TurnIndicatorManager : MonoBehaviour
{
    public TextMeshProUGUI turnIndicatorText; // Reference to the TextMeshProUGUI component

    void Start()
    {
        UpdateTurnIndicator(); // Initialize the turn indicator
    }

    public void UpdateTurnIndicator()
    {
        if (TurnManager.Instance != null)
        {
            int currentPlayer = TurnManager.Instance.currentPlayer;
            turnIndicatorText.text = $"Player {currentPlayer + 1}'s Turn!";
        }
        else
        {
            turnIndicatorText.text = "Waiting for TurnManager...";
        }
    }
}

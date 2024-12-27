using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ensure TextMeshPro is included for UI text updates

public class EndTurnButton : MonoBehaviour
{
    private Button _button;

    void Start()
    {
        _button = GetComponent<Button>();

        if (_button == null)
        {
            Debug.LogError("Button component not found on EndTurnButton GameObject.");
            return;
        }

        _button.onClick.AddListener(EndTurn); // Add the listener for the button
    }

    void Update()
    {
        if (TurnManager.Instance == null)
        {
            Debug.LogWarning("TurnManager.Instance is null. Waiting for initialization...");
            _button.interactable = false; // Disable the button until TurnManager is initialized
            return;
        }

        // Enable the button for the current active player
        _button.interactable = true;

        // Update button text to reflect the current player
        TextMeshProUGUI buttonText = _button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = $"End Turn (Player {TurnManager.Instance.currentPlayer + 1})";
        }
    }

    void EndTurn()
    {
        Debug.Log("Ending turn.");
        TurnManager.Instance.EndTurn();
    }
}

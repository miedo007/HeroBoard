using TMPro;
using UnityEngine;

public class UnitDetailsPanel : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI actionsText;

    public void UpdateUnitDetails(UnitBase unit, UnitTurnState turnState)
    {
        if (unit == null || turnState == null)
        {
            Debug.LogWarning("UpdateUnitDetails called with a null parameter.");
            ClearDetails();
            return;
        }

        healthText.text = $"Health: {unit.health}/{unit.maxHealth}";
        attackText.text = $"Attack: {unit.damage}";
        actionsText.text = $"Actions: {(turnState.hasMoved ? "Moved" : "Ready")}, {(turnState.hasAttacked ? "Attacked" : "Ready")}";
    }

    public void ClearDetails()
    {
        healthText.text = "Health:";
        attackText.text = "Attack:";
        actionsText.text = "Actions:";
    }

    public void UpdatePosition(Vector3 worldPosition)
    {
        if (Camera.main != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            transform.position = screenPosition;
        }
        else
        {
            Debug.LogError("Camera.main is null! Ensure there's a Main Camera in the scene.");
        }
    }
}

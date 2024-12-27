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
}

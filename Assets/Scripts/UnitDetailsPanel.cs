using TMPro;
using UnityEngine;

public class UnitDetailsPanel : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI actionsText;
    public RectTransform panelTransform;
    public float verticalOffset = 50f;  // Distance from the unit
    public float horizontalOffset = 20f;  // Extra horizontal spacing

    void Start()
    {
        // Ensure panelTransform is assigned
        if (panelTransform == null)
        {
            panelTransform = GetComponent<RectTransform>();
            if (panelTransform == null)
            {
                Debug.LogError("PanelTransform is not assigned and RectTransform could not be found!");
                return;
            }
        }
    }

    public void UpdateUnitDetails(UnitBase unit, UnitTurnState turnState, Vector3 unitWorldPosition)
    {
        if (unit == null || turnState == null)
        {
            ClearDetails();
            return;
        }

        // Update text details
        healthText.text = $"Health: {unit.health}/{unit.maxHealth}";
        attackText.text = $"Attack: {unit.damage}";
        actionsText.text = $"Actions: {(turnState.hasMoved ? "Moved" : "Ready")}, {(turnState.hasAttacked ? "Attacked" : "Ready")}";

        // Position the panel dynamically
        PositionPanel(unitWorldPosition);
    }

    public void ClearDetails()
    {
        healthText.text = "Health:";
        attackText.text = "Attack:";
        actionsText.text = "Actions:";
        panelTransform.position = new Vector3(-1000, -1000, 0); // Move off-screen
    }

    private void PositionPanel(Vector3 unitWorldPosition)
    {
        if (panelTransform == null)
        {
            Debug.LogError("PanelTransform is not assigned!");
            return;
        }

        // Convert unit's world position to screen position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(unitWorldPosition);
        Vector3 adjustedPosition = screenPosition + new Vector3(0, verticalOffset, 0); // Default to above

        // Adjust position dynamically based on proximity to screen edges
        if (screenPosition.y + panelTransform.rect.height + verticalOffset > Screen.height) // Near top
        {
            adjustedPosition = screenPosition + new Vector3(horizontalOffset, 0, 0); // Shift to the side
        }

        // Ensure the panel stays within screen bounds
        adjustedPosition.x = Mathf.Clamp(adjustedPosition.x, 0 + panelTransform.rect.width / 2, Screen.width - panelTransform.rect.width / 2);
        adjustedPosition.y = Mathf.Clamp(adjustedPosition.y, 0 + panelTransform.rect.height / 2, Screen.height - panelTransform.rect.height / 2);

        panelTransform.position = adjustedPosition;
    }
}

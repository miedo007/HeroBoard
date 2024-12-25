using UnityEngine;

public class UnitTurnState : MonoBehaviour
{
    public bool hasMoved = false; // Tracks if this unit has moved
    public bool hasAttacked = false; // Tracks if this unit has attacked

    private UnitHighlighter _highlighter;

    void Start()
    {
        _highlighter = GetComponent<UnitHighlighter>();
        UpdateHighlight();
    }

    public void ResetState()
    {
        hasMoved = false;
        hasAttacked = false;
        UpdateHighlight();
    }

    public void UpdateHighlight()
    {
        if (_highlighter != null)
        {
            bool canAct = !hasMoved || !hasAttacked;
            _highlighter.SetHighlight(canAct);
        }
    }
}

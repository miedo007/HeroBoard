using UnityEngine;

public class UnitHighlighter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _teamColor; // Store the unit's original team color

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _teamColor = _spriteRenderer.color; // Save the initial team color (blue or red)
        }
    }

    public void SetHighlight(bool isActive)
    {
        if (_spriteRenderer != null)
        {
            // Adjust brightness without changing the original team color
            _spriteRenderer.color = isActive ? _teamColor : _teamColor * 0.5f; // Dim if inactive
        }
    }
}

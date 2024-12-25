using UnityEngine;

public class UnitTurnState : MonoBehaviour
{
    public bool hasMoved = false; // Tracks if this unit has moved
    public bool hasAttacked = false; // Tracks if this unit has attacked

    public void ResetState()
    {
        hasMoved = false;
        hasAttacked = false;
    }
}

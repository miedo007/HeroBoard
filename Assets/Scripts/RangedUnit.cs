using UnityEngine;

public class RangedUnit : UnitBase
{
    public override void Attack(UnitBase target)
    {
        Debug.Log(unitName + " is using a ranged attack!");
        base.Attack(target); // Call the base attack logic
    }
}

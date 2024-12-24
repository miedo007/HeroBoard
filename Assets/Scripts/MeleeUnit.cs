using UnityEngine;

public class MeleeUnit : UnitBase
{
    public override void Attack(UnitBase target)
    {
        Debug.Log(unitName + " is using a melee attack!");
        base.Attack(target); // Call the base attack logic
    }
}

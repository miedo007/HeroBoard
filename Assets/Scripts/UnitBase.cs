using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public string unitName = "Unit"; // Name of the unit
    public int health = 100; // Current health points
    public int maxHealth = 100; // Maximum health points
    public int damage = 20; // Damage dealt per attack
    public int teamID; // Identifier for the unit's team (e.g., 0 for Player 1, 1 for Player 2)

    public Transform healthBar; // Reference to the health bar Transform

    public void TakeDamage(int amount)
    {
        // Reduce health by the damage amount
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth); // Ensure health doesn't drop below 0

        Debug.Log(unitName + " took " + amount + " damage. Remaining health: " + health);

        // Update the health bar to reflect the new health
        UpdateHealthBar();

        // Check if the unit has died
        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Debug.Log(unitName + " has died!");
        Destroy(gameObject); // Remove the unit from the game
    }

    public virtual void Attack(UnitBase target)
    {
        if (target != null)
        {
            Debug.Log(unitName + " is attacking " + target.unitName + " for " + damage + " damage.");
            target.TakeDamage(damage);
        }
        else
        {
            Debug.Log("No valid target to attack!");
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Calculate the health as a percentage of max health
            float healthPercentage = (float)health / maxHealth;

            // Update the X-axis of the scale while keeping Y and Z constant
            healthBar.localScale = new Vector3(healthPercentage, healthBar.localScale.y, healthBar.localScale.z);
        }
    }
}

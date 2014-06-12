using UnityEngine;
using System.Collections;

public class ColorData
{
    /// <summary>
    /// The color.
    /// </summary>
    public Color color;

    /// <summary>
    /// The color intensity. 
    /// Range of 0.0 - 1.0.
    /// </summary>
    public float intensity;

    public ColorData(Color color, float intensity)
    {
        this.color = color;
        this.intensity = intensity;
    }
}

/// <summary>
/// An object with which can be interacted.
/// </summary>
public class Unit : MonoBehaviour
{
    /// <summary>
    /// The player this unit belongs to.
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// The resources that are associated with this unit.
    /// </summary>
    public GameResources resources = new GameResources();

    /// <summary>
    /// How much damage this unit can sustain before dying.
    /// </summary>
    public float health = 100;

    /// <summary>
    /// How much damage this unit can do.
    /// </summary>
    public float damage = 0;

    /// <summary>
    /// Whether the unit is static.
    /// </summary>
    public bool isStatic = true;

    /// <summary>
    /// The tile on which this unit is located.
    /// </summary>
    public Tile tile;

    /// <summary>
    /// The base color.
    /// </summary>
    private Color baseColor;

    /// <summary>
    /// Whether the turn is completed.
    /// </summary>
    public bool turnDone = false;

    public void Start()
    {
        // Store the base color.
        baseColor = renderer.material.color;
    }

    /// <summary>
    /// Move to the given tile
    /// </summary>
    /// <param name="tile">The tile to move to</param>
    /// <returns>Whether the move succeeded</returns>
    public bool MoveTo(Tile tile)
    {
        this.tile = tile;

        // End turn.
        EndTurn();

        return true;
    }

    /// <summary>
    /// Ineract with the given unit.
    /// </summary>
    /// <param name="interactWith"></param>
    /// <returns></returns>
    public bool InteractWith(Unit unit)
    {
        // Prevent friendly fire.
        if (player == unit.player)
        {
            return false;
        }

        // Gather resources.
        player.resources += unit.resources * resources;

        // Damage units.
        unit.health -= damage;
        if (unit.health <= 0)
        {
            unit.Die();
        }

        // End turn.
        EndTurn();

        return true;
    }

    /// <summary>
    /// End the turn for this unit.
    /// </summary>
    private void EndTurn()
    {
        turnDone = true;
        SendMessage("SetColor", new ColorData(Color.white, 0.6f));
    }

    /// <summary>
    /// A new round started.
    /// </summary>
    public void OnRoundStart()
    {
        turnDone = false;
        SendMessage("ResetColor");
    }

    /// <summary>
    /// Set the color.
    /// </summary>
    /// <param name="colorData">The color to set it to</param>
    public void SetColor(ColorData colorData)
    {
        renderer.material.color = baseColor * (1 - colorData.intensity) + colorData.color * colorData.intensity;
    }

    /// <summary>
    /// Reset the color to the default for this object.
    /// </summary>
    public void ResetColor()
    {
        renderer.material.color = baseColor;
    }

    /// <summary>
    /// Called when the unit dies. This is responsible for destroying the object.
    /// </summary>
    public void Die()
    {
        Destroy(gameObject);
    }
}

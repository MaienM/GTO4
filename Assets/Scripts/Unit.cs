using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An object with which can be interacted.
/// </summary>
public class Unit : Colorable
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
    /// The range of this unit. 0 means the unit cannot move.
    /// </summary>
    public int range = 0;

    /// <summary>
    /// The tile on which this unit is located.
    /// </summary>
    public Tile tile
    {
        get
        {
            return _tile;
        }
        set
        {
            if (_tile != null)
            {
                _tile.Occupant = null;
            }
            _tile = value;
            if (_tile != null)
            {
                _tile.Occupant = this;
            }
        }
    }
    private Tile _tile;

    /// <summary>
    /// Whether the turn is completed.
    /// </summary>
    public bool turnDone = false;
    
    /// <summary>
    /// Move to the given tile
    /// </summary>
    /// <param name="tile">The tile to move to</param>
    /// <returns>Whether the move succeeded</returns>
    public bool MoveTo(Tile tile)
    {
        // Check whether the tile is in range.
        List<Tile> path = Tile.Pathfind(this.tile, tile, range);
        if (path == null)
        {
            return false;
        }

        // Move.
        StartCoroutine(MovePath(this.tile, tile));
        this.tile = tile;

        // End turn.
        EndTurn();

        return true;
    }

    private IEnumerator MovePath(Tile start, Tile end)
    {
        List<Tile> path = Tile.Pathfind(start, end, range);
        path.Remove(start);
        foreach (Tile t in path)
        {
            while ((transform.position - t.transform.position).magnitude > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.transform.position, 0.3f);
                yield return new WaitForFixedUpdate();
            }
        }
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
        SendMessage("AddColor", new ColorData("disable", Color.white, 0.6f, 10));
    }

    /// <summary>
    /// A new round started.
    /// </summary>
    public void OnRoundStart()
    {
        turnDone = false;
        SendMessage("RemoveColor", "disable");
    }

    /// <summary>
    /// Called when the unit dies. This is responsible for destroying the object.
    /// </summary>
    public void Die()
    {
        Destroy(gameObject);
    }
}

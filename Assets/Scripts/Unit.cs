using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MaienM.UnityUtils.CoreEx;

/// <summary>
/// An object with which can be interacted.
/// </summary>
public class Unit : Photon.MonoBehaviour 
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
    /// The cost to create one of this unit type.
    /// </summary>
    public GameResources cost = new GameResources();

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
    /// The interact range of this unit. 0 means the unit cannot interact.
    /// </summary>
    public int interactRange = 0;

    /// <summary>
    /// The tile on which this unit is located.
    /// </summary>
    public Tile tile;

    /// <summary>
    /// Offset from tile.
    /// </summary>
    public Vector3 offset = Vector3.zero;

    /// <summary>
    /// Whether the turn is completed.
    /// </summary>
    public bool turnDone = false;
    
    public void OnGUI()
    {
        GUIContent content = new GUIContent(health.ToString());
        GUIStyle style = GUI.skin.label;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos = new Vector2(screenPos.x, Screen.height - screenPos.y);
        GUI.Label(style.CalcSize(content).ToRectSize().Position(screenPos.ToRectPos(), Location.BOTTOM, 0.5f, 0), content);
    }

    public void SetOffset(Vector3 offset)
    {
        photonView.RPC("_SetOffset", PhotonTargets.AllBuffered, offset);
    }

    [RPC]
    public void _SetOffset(Vector3 offset)
    {
        this.offset = offset;
    }

    public void SetTile(Tile tile)
    {
        photonView.RPC("SetTile", PhotonTargets.AllBuffered, tile.Location);
    }

    [RPC]
    public void SetTile(Vector2 location)
    {
        // Get the tile.
        Tile tile = Utils.levelBuilder.GetTile(location);

        // Update occupant.
        if (this.tile != null)
        {
            this.tile.Occupant = null;
        }
        if (tile != null)
        {
            tile.Occupant = this;
        }

        // Update tile.
        this.tile = tile;
    }
    
    /// <summary>
    /// Move to the given tile
    /// </summary>
    /// <param name="tile">The tile to move to</param>
    /// <returns>Whether the move succeeded</returns>
    public bool MoveTo(Tile tile, Tile from = null)
    {
        Vector2 position = tile.Location;
        Vector2 fromPosition = (from != null) ? from.Location : this.tile.Location;
        photonView.RPC("MoveTo", PhotonTargets.Others, position, fromPosition);
        return MoveTo(position, fromPosition);
    }

    [RPC]
    public bool MoveTo(Vector2 position, Vector2 fromPosition)
    {
        Tile tile = Utils.levelBuilder.GetTile(position);
        Tile from = Utils.levelBuilder.GetTile(fromPosition);

        // Check whether the tile is in range.
        List<Tile> path = Tile.Pathfind(from, tile, range);
        if (path == null)
        {
            return false;
        }

        // Move.
        StartCoroutine(MovePath(from, tile));
        this.SetTile(tile);

        return true;
    }

    private IEnumerator MovePath(Tile start, Tile end)
    {
        // Get the path.
        List<Tile> path = Tile.Pathfind(start, end, range);
        path.Remove(start);

        // Start the movement animation.
        Utils.DoAnimator(gameObject, "Moving", true);
        foreach (Tile t in path)
        {
            Vector3 endPoint = t.transform.position + offset;

            // Look to the endpoint.
            transform.LookAt(endPoint);
            
            // Move to the endpoint.
            while ((transform.position - endPoint).magnitude > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint, 0.1f);
                yield return new WaitForFixedUpdate();
            }
        }

        // Finish the movement animation.
        Utils.DoAnimator(gameObject, "Moving", false);

        // End turn.
        OnRoundEnd();
    }

    /// <summary>
    /// Ineract with the given unit.
    /// </summary>
    /// <param name="interactWith"></param>
    /// <returns></returns>
    public bool InteractWith(Unit unit)
    {
        // Prevent friendly fire.
        if (unit.player == this.player)
        {
            return false;
        }

        photonView.RPC("InteractWith", PhotonTargets.Others, unit.tile.Location);
        InteractWith(unit.tile.Location);

        return true;
    }

    [RPC]
    public void InteractWith(Vector2 position)
    {
        Unit unit = Utils.levelBuilder.GetTile(position).Occupant;
        StartCoroutine(_InteractWith(unit));
    }

    private IEnumerator _InteractWith(Unit unit)
    {
        // Look at target.
        transform.LookAt(unit.tile.transform.position + offset);

        // Display animation.
        Utils.DoAnimator(gameObject, "Attack");
        yield return StartCoroutine(Utils.DoAnimatorWait(gameObject));
        Utils.DoAnimator(unit.gameObject, "Hit");

        // Damage units.
        unit.health -= damage;
        if (unit.health <= 0)
        {
            StartCoroutine(unit.Die());
        }

        // Gather resources.
        if (player != null && unit != null)
        {
            player.resources += unit.resources * resources;
        }

        // End turn.
        OnRoundEnd();
    }

    /// <summary>
    /// End the turn for this unit.
    /// </summary>
    public void OnRoundEnd()
    {
        if (range == 0)
        {
            return;
        }
        turnDone = true;
        SendMessage("AddColor", new ColorData("disable", Color.black, 0.6f, 10), SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// A new round started.
    /// </summary>s
    public void OnRoundStart()
    {
        turnDone = false;
        SendMessage("RemoveColor", "disable", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Called when the unit dies. This is responsible for destroying the object.
    /// </summary>
    public IEnumerator Die()
    {
        yield return StartCoroutine(Utils.DoAnimatorWait(gameObject));
        Utils.DoAnimator(gameObject, "Die");
        yield return new WaitForSeconds(1);
        Destroy();
        SendMessage("OnDie", SendMessageOptions.DontRequireReceiver);
    }

    [RPC]
    public void Destroy()
    {
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}

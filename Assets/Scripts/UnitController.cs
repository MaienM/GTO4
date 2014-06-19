using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour  
{
    /// <summary>
    /// The object with which to mark selected units.
    /// </summary>
    public GameObject marker;

    /// <summary>
    /// The unit we have selected.
    /// </summary>
    private Unit selectedUnit;

    /// <summary>
    /// Handles clicks on tiles, for unit movement/activation.
    /// </summary>
    /// <param name="tile">The clicked tile</param>
    public void OnTileClick(Tile tile)
    {
        if (Utils.gameController.IsEnded)
        {
            return;
        }

        // If we don't have a selection already, check if there is something to select.
        if (selectedUnit == null)
        {
            if (tile.Occupant != null && !tile.Occupant.turnDone && tile.Occupant.range > 0)
            {
                selectedUnit = tile.Occupant;
                marker.SetActive(true);
                marker.transform.position = selectedUnit.transform.position;

                // Highlight all available tiles.
                IEnumerable<Tile> tiles = Tile.Neighbourhood(tile, tile.Occupant.range);
                foreach (Tile t in tiles)
                {
                    t.SendMessage("AddColor", new ColorData("range", Color.red, 0.5f, 1));
                    if (t.Occupant)
                    {
                        t.Occupant.SendMessage("AddColor", new ColorData("range", Tile.Distance(tile, t) <= selectedUnit.interactRange ? Color.green : Color.red, 0.5f, 10), SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }

        // If we do have a selection, check what we clicked on.
        else
        {
            // If we click on an empty tile, try to move there.
            if (tile.Occupant == null)
            {
                if (selectedUnit.MoveTo(tile))
                {
                    ClearSelection();
                }
            }

            // If we click on the unit itself, cancel.
            else if (tile.Occupant == selectedUnit)
            {
                ClearSelection();
            }

            // If there is a unit on the tile, try to interact.
            else
            {
                if (Tile.Distance(tile, selectedUnit.tile) <= selectedUnit.interactRange && selectedUnit.InteractWith(tile.Occupant))
                {
                    ClearSelection();
                }
            }
        }
    }

    public void onDrop(GameObject prefab)
    {
        // Find the tile on which we dropped something.
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // Get the tile.
            Tile tile = hit.transform.gameObject.GetComponent<Tile>();
            if (tile == null || tile.Occupant != null)
            {
                return;
            }

            // Get the unit script of the prefab.
            Unit prefabUnit = prefab.GetComponent<Unit>();

            // Check whether the player can afford this unit.
            if (!(Utils.playerController.resources >= prefabUnit.cost))
            {
                return;
            }

            // Check whether the unit can be placed there.
            if (Tile.Pathfind(Utils.playerController.home.tile, tile, prefabUnit.range) == null)
            {
                return;
            }

            // Place the unit.
            Utils.playerController.resources -= prefabUnit.cost;
            GameObject gameobj = PhotonNetwork.Instantiate(prefab.name, Utils.playerController.home.tile.transform.position + prefab.transform.position, prefab.transform.rotation, 0) as GameObject;
            Unit unit = gameobj.GetComponent<Unit>();
            unit.player = Utils.playerController;
            unit.SetOffset(prefab.transform.position);
            unit.MoveTo(tile, Utils.playerController.home.tile);
        }
    }

    private void ClearSelection()
    {
        marker.SetActive(false);
        selectedUnit = null;
        Utils.BroadcastMessageAll("RemoveColor", "range");
    }
}

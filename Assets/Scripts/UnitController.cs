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
    public Unit selectedUnit;

    public void OnTileClick(Tile tile)
    {
        // If we don't have a selection already, check if there is something to select.
        if (selectedUnit == null)
        {
            if (tile.Occupant != null && tile.Occupant.range > 0)
            {
                selectedUnit = tile.Occupant;
                marker.SetActive(true);
                marker.transform.position = selectedUnit.transform.position;

                // Highlight all available tiles.
                IEnumerable<Tile> tiles = Tile.Neighbourhood(tile, tile.Occupant.range);
                foreach (Tile t in tiles)
                {
                    t.SendMessage("AddColor", new ColorData("range", Color.red, 0.5f, 1));
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
                    marker.SetActive(false);
                    selectedUnit = null;
                }
            }

            // If we click on the unit itself, cancel.
            else if (tile.Occupant == selectedUnit)
            {
                marker.SetActive(false);
                selectedUnit = null;

                // Un-highlight all available tiles.
                IEnumerable<Tile> tiles = Tile.Neighbourhood(tile, tile.Occupant.range);
                foreach (Tile t in tiles)
                {
                    t.SendMessage("RemoveColor", "range");
                }
            }

            // If there is a unit on the tile, try to interact.
            else
            {
                if (selectedUnit.InteractWith(tile.Occupant))
                {
                    marker.SetActive(false);
                    selectedUnit = null;
                }
            }
        }
    }
}

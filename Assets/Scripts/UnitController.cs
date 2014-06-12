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
    /// A list of unit that can be built.
    /// </summary>
    public List<GameObject> buildable = new List<GameObject>();

    private List<HoverButton> buttons = null;

    /// <summary>
    /// Handles the GUI for adding units.
    /// </summary>
    public void OnGUI()
    {
        if (!PrefabRenderer.Done)
        {
            return;
        }

        if (buttons == null)
        {
            buttons = new List<HoverButton>();
            int i = 1;
            foreach (GameObject prefab in buildable)
            {
                buttons.Add(new HoverButton(new Rect(20, Screen.height - 110 * i, 100, 100), new GUIContent(PrefabRenderer.RenderedTextures[prefab]), GUI.skin.button, prefab));
                i++;
            }
        }

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            GUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    foreach (HoverButton hob in buttons)
                    {
                        hob.OnGUI();
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }

    /// <summary>
    /// Handles clicks on tiles, for unit movement/activation.
    /// </summary>
    /// <param name="tile">The clicked tile</param>
    public void OnTileClick(Tile tile)
    {
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
                        t.Occupant.SendMessage("AddColor", new ColorData("range", Tile.Distance(tile, t) == 1 ? Color.green : Color.red, 0.5f, 10));
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
                if (selectedUnit.InteractWith(tile.Occupant))
                {
                    ClearSelection();
                }
            }
        }
    }

    public void onUnitDrop(GameObject prefab)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Tile t = hit.transform.gameObject.GetComponent<Tile>();
            if (t == null || t.Occupant != null)
            {
                return;
            }
            PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            GameResources pr = pc.resources;
            GameResources uc = prefab.GetComponent<Unit>().cost;
            if (pr.IRON >= uc.IRON && pr.WOOD >= uc.WOOD && pr.STONE >= uc.STONE)
            {
                pc.resources -= uc;
                GameObject.Instantiate(prefab, t.transform.position, Quaternion.identity);
            }
        }
    }

    private void ClearSelection()
    {
        marker.SetActive(false);
        selectedUnit = null;
        Utils.BroadcastMessageAll("RemoveColor", "range");
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Tile : Photon.MonoBehaviour  
{
    /// <summary>
    /// The X position of this tile.
    /// </summary>
    public int X;

    /// <summary>
    /// The Z position of this tile.
    /// </summary>
    public int Z;

    /// <summary>
    /// The object currently occupying this tile.
    /// </summary>
    public Unit Occupant;

    /// <summary>
    /// The neighbours of this tile.
    /// </summary>
    public List<Tile> Neighbours = new List<Tile>();

    /// <summary>
    /// The coordinates -
    /// </summary>
    public Vector2 Location
    {
        get
        {
            return new Vector2(X, Z);
        }
    }

    /// <summary>
    /// Mouse click event.
    /// </summary>
    public void OnMouseDown()
    {
        gameObject.transform.root.SendMessage("OnTileClick", this, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Mouse enter event.
    /// </summary>
    public void OnMouseEnter()
    {
        SendMessage("AddColor", new ColorData("hover", Color.green, 0.5f, 10));
    }

    /// <summary>
    /// Mouse leave event.
    /// </summary>
    public void OnMouseExit()
    {
        SendMessage("RemoveColor", "hover");
    }

    /// <summary>
    /// Calculates the optimal distance between two tiles.
    /// This is a optimistic estimation, which assumes you can travel in a straight line.
    /// </summary>
    /// <param name="a">The first tile</param>
    /// <param name="b">The second tile</param>
    /// <returns>The distance between these tiles</returns>
    public static int Distance(Tile a, Tile b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Z - b.Z);
    }

    /// <summary>
    /// Find the shortest path between two tiles.
    /// 
    /// This is NOT optimized, but that is not important for our purposes.
    /// </summary>
    /// <param name="start">The start tile</param>
    /// <param name="end">The end tile</param>
    /// <param name="steps">The max remaining steps</param>
    /// <returns>The shortest path, if any. Null if no path exists.</returns>
    public static List<Tile> Pathfind(Tile start, Tile end, int steps)
    {
        // We're out of range, abort this branch.
        if (steps < Tile.Distance(start, end))
        {
            return null;
        }

        List<Tile> path;
        List<List<Tile>> paths = new List<List<Tile>>();
        foreach (Tile neighbour in start.Neighbours)
        {
            // If this is the target, return.
            if (neighbour == end)
            {
                path = new List<Tile>();
                path.Add(end);
                return path;
            }

            // If something is blocking the path, ignore it.
            if (neighbour.Occupant != null)
            {
                continue;
            }

            // Get the shortest path through this neighbour.
            path = Tile.Pathfind(neighbour, end, steps - 1);
            if (path != null)
            {
                paths.Add(path);
            }
        }
        
        // If we didn't find any paths, return null.
        if (paths.Count == 0)
        {
            return null;
        }

        // Determine the shortest path, insert ourselves, return.
        path = paths.OrderBy(t => t.Count).First();
        path.Insert(0, start);
        return path;
    }

    /// <summary>
    /// Get all tiles in the neighbourhood of tile.
    /// </summary>
    /// <param name="tile">The start tile</param>
    /// <param name="range">The max range</param>
    /// <returns>All tiles in the neighbourhood</returns>
    public static IEnumerable<Tile> Neighbourhood(Tile tile, int range)
    {
        if (range == 1)
        {
            return tile.Neighbours;
        }
        else
        {
            List<Tile> tiles = new List<Tile>();
            foreach (Tile neighbour in tile.Neighbours)
            {
                tiles.Add(neighbour);
                if (neighbour.Occupant == null)
                {
                    foreach (Tile t in Tile.Neighbourhood(neighbour, range - 1))
                    {
                        tiles.Add(t);
                    }
                }
            }
            return tiles;
        }
    }
}

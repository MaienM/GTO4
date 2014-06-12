using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour 
{
    /// <summary>
    /// The size of the level. 
    /// </summary>
    public int size = 10;

    /// <summary>
    /// The prefab used for the floor of the level.
    /// </summary>
    public GameObject floorPrefab;

    /// <summary>
    /// The grid of the level.
    /// </summary>
    public Tile[,] grid;

    /// <summary>
    /// The other objects to spawn in the world.
    /// </summary>
    public GameObject[] prefabs;

	public void Start() 
	{
        // Create vectors to take one step in either direction.
        Vector3 step = floorPrefab.transform.localScale * 10;
        Vector3 stepX = new Vector3(step.x, 0, 0);
        Vector3 stepZ = new Vector3(0, 0, step.z);

        // Create the grid.
        grid = new Tile[size, size];

        // Create the floors
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Vector3 position = x * stepX + z * stepZ;
                GameObject go = Instantiate(floorPrefab, position, Quaternion.identity) as GameObject;
                go.transform.parent = gameObject.transform;
                Tile tile = go.AddComponent<Tile>();
                tile.X = x;
                tile.Z = z;
                grid[x, z] = tile;
            }
        }

        // Spawn extras.
        foreach (GameObject prefab in prefabs)
        {
            GameObject go = (GameObject)Instantiate(prefab);
            int x, z;
            do
            {
                x = Random.Range(0, size);
                z = Random.Range(0, size);
            } while (grid[x, z].Occupant != null);
            grid[x, z].Occupant = go;
            go.transform.position = grid[x, z].gameObject.transform.position;
        }
	}
}

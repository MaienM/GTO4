using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : Photon.MonoBehaviour  
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

    public void Awake()
    {
        ConsoleCommandsRepository ccr = ConsoleCommandsRepository.Instance;
        ccr.RegisterCommand("dumpLevel", PrintBoard);
    }

    public void PrintBoard(params string[] args)
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            string line = "";
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                line += grid[x, y].Occupant == null ? "[ ]" : "[x]";
            }
            Debug.Log(line);
        }
    }

    public void OnJoinedRoom()
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
                Vector3 position = x * stepX + z * stepZ - (size - 1) * 0.5f * step;
                GameObject go = Instantiate(floorPrefab, position, Quaternion.identity) as GameObject;
                go.transform.parent = gameObject.transform;
                Tile tile = go.GetComponent<Tile>();
                tile.X = x;
                tile.Z = z;
                grid[x, z] = tile;
            }
        }

        // Introduce the tiles to their neighbours.
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Tile tile = grid[x, z];
                if (x > 0)
                    tile.Neighbours.Add(grid[x - 1, z]);
                if (x < size - 1)
                    tile.Neighbours.Add(grid[x + 1, z]);
                if (z > 0)
                    tile.Neighbours.Add(grid[x, z - 1]);
                if (z < size - 1)
                    tile.Neighbours.Add(grid[x, z + 1]);
            }
        }

        // Spawn extras.
        if (PhotonNetwork.isMasterClient)
        {
            foreach (GameObject prefab in prefabs)
            {
                int x, z;
                do
                {
                    x = Random.Range(0, size);
                    z = Random.Range(0, size);
                } while (grid[x, z].Occupant != null);
                Tile tile = grid[x, z];
                GameObject go = PhotonNetwork.Instantiate(prefab.name, tile.gameObject.transform.position, prefab.transform.rotation, 0) as GameObject;
                go.GetComponent<Unit>().SetTile(tile);
            }
        }

        Utils.BroadcastMessageAll("LevelBuilt", this);
	}

    public Tile GetTile(Vector2 position)
    {
        return grid[(int)position.x, (int)position.y];
    }
}

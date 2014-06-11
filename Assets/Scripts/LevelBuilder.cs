using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour 
{
    /// <summary>
    /// The radius of the level. 
    /// 
    /// The total level size will be 2 * radius + 1 by 2 * radius + 1.
    /// </summary>
    public int radius = 10;

    /// <summary>
    /// The prefab used for the floor of the level.
    /// </summary>
    public GameObject floorPrefab;

    /// <summary>
    /// The grid of the level.
    /// </summary>
    public GameObject[,] grid;

    /// <summary>
    /// One step in the x direction.
    /// </summary>
    private Vector3 stepX;

    /// <summary>
    /// One step in the z direction.
    /// </summary>
    private Vector3 stepZ;

	public void Start() 
	{
        // Create vectors to take one step in either direction.
        Vector3 step = floorPrefab.transform.localScale * 10;
        stepX = new Vector3(step.x, 0, 0);
        stepZ = new Vector3(0, 0, step.z);

        // Create the floors
        CreateFloor(Vector3.zero);
        for (int i = 0; i < radius; i++)
        {
            for (int j = 0; j < radius + 1; j++)
            {
                CreateFloor(stepX * (i + 1) - stepZ * j);
                CreateFloor(-stepX * (i + 1) + stepZ * j);
                CreateFloor(stepZ * (i + 1) + stepX * j);
                CreateFloor(-stepZ * (i + 1) - stepX * j);
            }
        }

        // Create the grid.
        grid = new GameObject[radius * 2 + 1, radius * 2 + 1];
	}

    /// <summary>
    /// Create a new floor tile at the given position.
    /// </summary>
    /// <param name="position">The position at which to create the floor tile</param>
    /// <returns>The new floor tile</returns>
    private GameObject CreateFloor(Vector3 position)
    {
        GameObject go = Instantiate(floorPrefab, position, Quaternion.identity) as GameObject;
        go.transform.parent = gameObject.transform;
        return go;
    }

    /// <summary>
    /// Get the position of a grid tile.
    /// </summary>
    /// <param name="x">The x index of the tile</param>
    /// <param name="z">The z index of the tile</param>
    /// <returns>The position of this tile in the world</returns>
    public Vector3 GetGridPosition(int x, int z)
    {
        return (x - radius - 1) * stepX + (z - radius - 1) * stepZ;
    }
}

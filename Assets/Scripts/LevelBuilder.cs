using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour 
{
    public int size = 10;
    public GameObject floorPrefab;
    public List<GameObject> objectPrefabs;
    public GameObject[,] grid;

	public void Start() 
	{
        // Create vectors to take one step in either direction.
        Vector3 step = floorPrefab.transform.localScale * 10;
        step.y = 0;
        Vector3 stepx = new Vector3(step.x, 0, 0);
        Vector3 stepz = new Vector3(0, 0, step.z);

        // Create the floors
        Instantiate(floorPrefab, Vector3.zero);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size + 1; j++)
            {
                Instantiate(floorPrefab, stepx * (i + 1) - stepz * j);
                Instantiate(floorPrefab, -stepx * (i + 1) + stepz * j);
                Instantiate(floorPrefab, stepz * (i + 1) + stepx * j);
                Instantiate(floorPrefab, -stepz * (i + 1) - stepx * j);
            }
        }

        // Create the other objects.
        grid = new GameObject[size * 2 + 1, size * 2 + 1];
        foreach (GameObject prefab in objectPrefabs)
        {
            for (int i = 0; i < size; i++)
            {
                int x, z;
                do
                {
                    x = Random.Range(0, size * 2 + 1);
                    z = Random.Range(0, size * 2 + 1);
                }
                while (grid[x, z] != null);
                grid[x, z] = Instantiate(prefab, (x - size) * stepx + (z - size) * stepz);
            }
        }
	}

    new public GameObject Instantiate(GameObject prefab, Vector3 position)
    {
        GameObject go = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        go.transform.parent = gameObject.transform;
        return go;
    }
}

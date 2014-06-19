using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour 
{
    public List<Material> materials;

	public void Start() 
	{
        renderer.material = materials[Random.Range(0, materials.Count)];
        GetComponent<Colorable>().baseColor = renderer.material.color;
	}
}

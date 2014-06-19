using System.Collections.Generic;
using UnityEngine;

public class RandomTexture : MonoBehaviour 
{
    public List<Texture> textures;

	public void Start() 
	{
        renderer.material.mainTexture = textures[Random.Range(0, textures.Count)];
	}
}

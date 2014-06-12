using System;
using UnityEngine;

public class Tile : MonoBehaviour 
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
    public GameObject Occupant
    {
        get
        {
            return _occupant;
        }
        set
        {
            _occupant = value;
            Unit u = value.GetComponent<Unit>();
            if (u != null)
            {
                u.tile = this;
            }
        }
    }
    private GameObject _occupant;

    /// <summary>
    /// The base color.
    /// </summary>
    private Color baseColor;

    public void Start()
    {
        baseColor = gameObject.renderer.material.color;
    }

    public void OnMouseDown()
    {
        gameObject.transform.root.SendMessage("OnTileClick", this, SendMessageOptions.DontRequireReceiver);
    }

    public void OnMouseEnter()
    {
        gameObject.renderer.material.color = Color.white;
    }

    public void OnMouseExit()
    {
        gameObject.renderer.material.color = baseColor;
    }
}

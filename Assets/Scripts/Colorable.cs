using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorData
{
    /// <summary>
    /// The name of the colorization.
    /// </summary>
    public string name;

    /// <summary>
    /// The color.
    /// </summary>
    public Color color;

    /// <summary>
    /// The color intensity. 
    /// Range of 0.0 - 1.0.
    /// </summary>
    public float intensity;

    /// <summary>
    /// The color prioroty. 
    /// Higher means more important.
    /// </summary>
    public int priority;

    public ColorData(string name, Color color, float intensity, int priority)
    {
        this.name = name;
        this.color = color;
        this.intensity = intensity;
        this.priority = priority;
    }
}

public class Colorable : MonoBehaviour
{
    /// <summary>
    /// The base color.
    /// </summary>
    private Color baseColor;

    /// <summary>
    /// The currently applied colors.
    /// </summary>
    private List<ColorData> colors = new List<ColorData>();

    /// <summary>
    /// Store the base color.
    /// </summary>
    public void Start()
    {
        baseColor = renderer.material.color;
    }

    /// <summary>
    /// Add a color.
    /// </summary>
    /// <param name="colorData">The color to add</param>
    public void AddColor(ColorData colorData)
    {
        colors.Add(colorData);
        colors.OrderBy(cd => cd.intensity);
        ApplyColor(colors.Last());
    }

    /// <summary>
    /// Apply a color.
    /// </summary>
    /// <param name="colorData">The color to apply</param>
    private void ApplyColor(ColorData colorData)
    {
        renderer.material.color = baseColor * (1 - colorData.intensity) + colorData.color * colorData.intensity;
    }

    /// <summary>
    /// Remove a color.
    /// </summary>
    /// <param name="name">The color name to remove</param>
    public void RemoveColor(string name)
    {
        colors.RemoveAll(cd => cd.name == name);
        if (colors.Count() > 0)
        {
            ApplyColor(colors.Last());
        }
        else
        {
            ResetColor();
        }
    }

    /// <summary>
    /// Reset the color to the default for this object.
    /// </summary>
    public void ResetColor()
    {
        colors.Clear();
        renderer.material.color = baseColor;
    }
}

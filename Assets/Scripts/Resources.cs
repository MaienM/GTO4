using System;
using System.Linq;
using System.Collections.Generic;

public enum ResourceType
{
    STONE,
    WOOD,
    IRON
}

public class Resources : Dictionary<ResourceType, float>
{
    /// <summary>
    /// Setup a new resources with all resources set to the given value.
    /// </summary>
    /// <param name="def"></param>
    public Resources(float def = 0)
    {
        SetAll(def);
    }

    /// <summary>
    /// Create a new resources that is a copy of another resources
    /// </summary>
    /// <param name="other">The resources to copy</param>
    public Resources(Resources other)
    {
        SetAll(0);
        foreach (KeyValuePair<ResourceType, float> pair in other)
        {
            this[pair.Key] = pair.Value;
        }
    }

    /// <summary>
    /// Set all resource type values to the given values.
    /// </summary>
    /// <param name="val">The value to set vor all resources</param>
    public void SetAll(float val)
    {
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>())
        {
            this[type] = val;
        }
    }

    #region Operands
    static public Resources operator +(Resources a, Resources b)
    {
        Resources c = new Resources(a);
        foreach (KeyValuePair<ResourceType, float> pair in b)
        {
            c[pair.Key] += pair.Value;
        }
        return c;
    }

    static public Resources operator -(Resources a, Resources b)
    {
        Resources c = new Resources(a);
        foreach (KeyValuePair<ResourceType, float> pair in b)
        {
            c[pair.Key] -= pair.Value;
        }
        return c;
    }

    static public Resources operator *(Resources a, Resources b)
    {
        Resources c = new Resources(a);
        foreach (KeyValuePair<ResourceType, float> pair in b)
        {
            c[pair.Key] *= pair.Value;
        }
        return c;
    }
    #endregion
}

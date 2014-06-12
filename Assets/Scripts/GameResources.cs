using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/*
[Serializable]
public enum GameResourceType
{
    STONE,
    WOOD,
    IRON
}


[Serializable]
public class GameResources : UnityEngine.Object
{
    public List<GameResourceType> Keys = new List<GameResourceType>();
    public List<float> Values = new List<float>();    

    /// <summary>
    /// Setup a new resources with all resources set to the given value.
    /// </summary>
    /// <param name="def"></param>
    public GameResources(float def = 0)
    {
        SetAll(def);
    }

    /// <summary>
    /// Create a new resources that is a copy of another resources
    /// </summary>
    /// <param name="other">The resources to copy</param>
    public GameResources(GameResources other)
    {
        SetAll(0);
        foreach (KeyValuePair<GameResourceType, float> pair in other)
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
        foreach (GameResourceType type in Enum.GetValues(typeof(GameResourceType)).Cast<GameResourceType>())
        {
            this[type] = val;
        }
    }

    #region Operands
    static public GameResources operator +(GameResources a, GameResources b)
    {
        GameResources c = new GameResources(a);
        foreach (KeyValuePair<GameResourceType, float> pair in b)
        {
            c[pair.Key] += pair.Value;
        }
        return c;
    }

    static public GameResources operator -(GameResources a, GameResources b)
    {
        GameResources c = new GameResources(a);
        foreach (KeyValuePair<GameResourceType, float> pair in b)
        {
            c[pair.Key] -= pair.Value;
        }
        return c;
    }

    static public GameResources operator *(GameResources a, GameResources b)
    {
        GameResources c = new GameResources(a);
        foreach (KeyValuePair<GameResourceType, float> pair in b)
        {
            c[pair.Key] *= pair.Value;
        }
        return c;
    }
    #endregion

    #region Dictionary
    public float this[GameResourceType key]
    {
        get
        {
            if (!Keys.Contains(key))
            {
                throw new KeyNotFoundException();
            }
            return Values[Keys.IndexOf(key)];
        }
        set
        {
            if (Keys.Contains(key))
            {
                Values[Keys.IndexOf(key)] = value;
            }
            else
            {
                Keys.Add(key);
                Values.Add(value);
            }
        }
    }

    public IEnumerator<KeyValuePair<GameResourceType, float>> GetEnumerator()
    {
        List<KeyValuePair<GameResourceType, float>> pairs = new List<KeyValuePair<GameResourceType, float>>();

        for (int i = 0; i < Keys.Count; i++)
        {
            pairs.Add(new KeyValuePair<GameResourceType, float>(Keys[i], Values[i]));
        }

        return pairs.GetEnumerator();
    }
    #endregion
}
*/

[Serializable]
public class GameResources
{
    public float IRON = 0;
    public float STONE = 0;
    public float WOOD = 0;

    public GameResources()
    {
    }

    public GameResources(GameResources other)
    {
        IRON = other.IRON;
        STONE = other.STONE;
        WOOD = other.WOOD;
    }

    static public GameResources operator +(GameResources a, GameResources b)
    {
        GameResources c = new GameResources(a);
        c.IRON = a.IRON + b.IRON;
        c.STONE = a.STONE + b.STONE;
        c.WOOD = a.WOOD + b.WOOD;
        return c;
    }

    static public GameResources operator -(GameResources a, GameResources b)
    {
        GameResources c = new GameResources(a);
        c.IRON = a.IRON - b.IRON;
        c.STONE = a.STONE - b.STONE;
        c.WOOD = a.WOOD - b.WOOD;
        return c;
    }

    static public GameResources operator *(GameResources a, GameResources b)
    {
        GameResources c = new GameResources(a);
        c.IRON = a.IRON * b.IRON;
        c.STONE = a.STONE * b.STONE;
        c.WOOD = a.WOOD * b.WOOD;
        return c;
    }
}
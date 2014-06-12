using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Broadcast a message to all game objects in the scene.
    /// </summary>
    /// <param name="message"></param>
    public static void BroadcastMessageAll(string methodName, object value = null)
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            go.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Send a message to all objects with a certain tag.
    /// </summary>
    /// <param name="tag">The tag</param>
    /// <param name="methodName">The method name to call</param>
    /// <param name="value">The value to pass as argument</param>
    public static void SendMessageToTag(string tag, string methodName, object value=null)
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(tag))
        {
            go.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
        }
    }
}

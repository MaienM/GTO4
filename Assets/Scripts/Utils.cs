using System.Collections;
using UnityEngine;

public static class Utils
{
    public static GameController gameController
    {
        get
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    public static LevelBuilder levelBuilder
    {
        get
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelBuilder>();
        }
    }

    public static UnitController unitController
    {
        get
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<UnitController>();
        }
    }

    public static PlayerController playerController
    {
        get
        {
            return GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerController>();
        }
    }

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
    /// Broadcast a message to all game objects in the scene.
    /// </summary>
    /// <param name="message"></param>
    public static void BroadcastMessageMine(string methodName, object value = null)
    {
        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            PhotonView view = go.GetComponent<PhotonView>();
            if (view != null && view.isMine)
            {
                go.SendMessage(methodName, value, SendMessageOptions.DontRequireReceiver);
            }
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

    public static void DoAnimator(GameObject source, string trigger)
    {
        Animator anim = source.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger(trigger);
        }
    }
    public static void DoAnimator(GameObject source, string key, bool value)
    {
        Animator anim = source.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool(key, value);
        }
    }
    public static void DoAnimator(GameObject source, string key, int value)
    {
        Animator anim = source.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetInteger(key, value);
        }
    }
    public static void DoAnimator(GameObject source, string key, float value)
    {
        Animator anim = source.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetFloat(key, value);
        }
    }
    public static IEnumerator DoAnimatorWait(GameObject source, string desiredState = "Idle")
    {
        Animator anim = source.GetComponent<Animator>();
        if (anim != null)
        {
            // Wait for the animator to start.
            while (anim.GetCurrentAnimatorStateInfo(0).IsName("Base." + desiredState))
            {
                yield return new WaitForFixedUpdate();
            }

            // Wait for it to end.
            while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Base." + desiredState))
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

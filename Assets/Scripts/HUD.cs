using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
    /// <summary>
    /// The player this HUD belongs to.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// The player controller.
    /// </summary>
    private PlayerController pc;

    public void Setup()
    {
        // Get the player controller.
        pc = player.GetComponent<PlayerController>();
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            GUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                {
                    //GUILayout.Label(pc.resources[ResourceType.IRON].ToString());
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }
}

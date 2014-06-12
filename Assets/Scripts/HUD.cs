using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class HUD : MonoBehaviour 
{
    /// <summary>
    /// The game controller game object.
    /// </summary>
    public GameObject controller;

    /// <summary>
    /// The game controller.
    /// </summary>
    private GameController gc;

    /// <summary>
    /// The player this HUD belongs to.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// The player controller.
    /// </summary>
    private PlayerController pc;

    public void Start()
    {
        gc = controller.GetComponent<GameController>();
        pc = player.GetComponent<PlayerController>();
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width * 0.1f, Screen.height));
        {
            GUILayout.BeginVertical();
            {
                // Resources.
                GUILayout.Label("WOOD: " + pc.resources.WOOD);
                GUILayout.Label("STONE: " + pc.resources.STONE);
                GUILayout.Label("IRON: " + pc.resources.IRON);

                // Spawn units.


                if (GUILayout.Button("End round"))
                {
                    gc.EndRound();
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }
}

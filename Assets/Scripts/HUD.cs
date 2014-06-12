using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class HUD : MonoBehaviour 
{
    /// <summary>
    /// The game controller.
    /// </summary>
    private GameController gc;

    /// <summary>
    /// The player controller.
    /// </summary>
    private PlayerController pc;

    /// <summary>
    /// The texture of the minimap.
    /// </summary>
    public RenderTexture minimapTexture;

    public void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
                //GUILayout

                if (GUILayout.Button("End round"))
                {
                    gc.EndRound();
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();

        GUI.DrawTexture(new Rect(Screen.width * 0.85f, Screen.height * 0.85f, Screen.width * 0.95f, Screen.height * 0.95f), minimapTexture);
    }
}

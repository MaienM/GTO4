using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MaienM.UnityUtils.CoreEx;

public class HUD : MonoBehaviour 
{
    /// <summary>
    /// The texture of the minimap.
    /// </summary>
    public RenderTexture minimapTexture;

    /// <summary>
    /// A list of unit that can be built.
    /// </summary>
    public List<GameObject> buildable = new List<GameObject>();

    /// <summary>
    /// The hover buttons that are used for the buildable objects.
    /// </summary>
    private List<HoverButton> buildButtons = null;

    /// <summary>
    /// The size of the minimap.
    /// </summary>
    public int MinimapSize = 100;

    public void Start()
    {
        StartCoroutine(InitButtons());

        // Setup the minimap.
        minimapTexture = new RenderTexture(MinimapSize, MinimapSize, 32, RenderTextureFormat.RGB565);
        Camera mmc = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Camera>();
        mmc.targetTexture = minimapTexture;
        mmc.orthographicSize = Utils.levelBuilder.size * 1.1f;
    }

    /// <summary>
    /// Initialize the hover buttons that are used for the buildable objects.
    /// </summary>
    /// <returns></returns>
    public IEnumerator InitButtons()
    {
        while (!PrefabRenderer.Done)
        {
            yield return new WaitForSeconds(0.2f);
        }

        buildButtons = new List<HoverButton>();
        foreach (GameObject prefab in buildable)
        {
            buildButtons.Add(new HoverButton(new GUIContent(PrefabRenderer.RenderedTextures[prefab]), new GUIStyle(), prefab));
        }
    }

    public void OnGUI()
    {
        if (Utils.gameController.IsEnded)
        {
            DrawEnd();
        }
        else
        {
            DrawHUD();
        }
    }
        
    public void DrawHUD()
    {
        GUI.enabled = Utils.gameController.IsCurrent;
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        {
            GUILayout.BeginVertical();
            {
                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                {
                    // Resources.
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label("WOOD: " + Utils.playerController.resources.WOOD);
                        GUILayout.Label("STONE: " + Utils.playerController.resources.STONE);
                        GUILayout.Label("IRON: " + Utils.playerController.resources.IRON);
                    }
                    GUILayout.EndVertical();

                    GUILayout.FlexibleSpace();

                    // Building units.
                    if (buildButtons != null)
                    {
                        foreach (HoverButton hob in buildButtons)
                        {
                            // Render the button.
                            GameObject prefab = (GameObject)hob.value;
                            Unit unit = prefab.GetComponent<Unit>();
                            GUI.enabled = Utils.gameController.IsCurrent && unit.cost <= Utils.playerController.resources;
                            switch (hob.OnGUI())
                            {
                                case EventType.mouseDown:
                                    foreach (Tile t in Tile.Neighbourhood(Utils.playerController.home.tile, unit.range))
                                    {
                                        t.SendMessage("AddColor", new ColorData("placement", Color.blue, 0.6f, 10));
                                    }
                                    break;

                                case EventType.mouseUp:
                                    GameObject.FindGameObjectWithTag("GameController").SendMessage("onDrop", prefab);
                                    Utils.BroadcastMessageAll("RemoveColor", "placement");
                                    break;

                                case EventType.mouseMove:
                                    GUIContent cont = new GUIContent(string.Format("Cost:\n\nWood: {0}\nStone: {1}\nIron: {2}\n\n" +
                                                                                   "Gathering efficiency:\n\nWood: {3}\nStone: {4}\nIron: {5}\n\n" +
                                                                                   "Stats:\n\nHealth: {6}\nDamage: {7}\nMovement: {8}\nInteract range: {9}",
                                                                                   unit.cost.WOOD, unit.cost.STONE, unit.cost.IRON, 
                                                                                   unit.resources.WOOD, unit.resources.STONE, unit.resources.IRON, 
                                                                                   unit.health, unit.damage, unit.range, unit.interactRange));
                                    GUIStyle style = GUI.skin.box;
                                    GUI.Label(style.CalcSize(cont).ToRectSize().Position(GUILayoutUtility.GetLastRect(), Location.BOTTOM, 0.5f, 0), cont, style);
                                    break;
                            }

                            // Render the label.
                            Rect buttonRect = GUILayoutUtility.GetLastRect();
                            GUIContent content = new GUIContent(prefab.name);
                            Rect labelRect = GUI.skin.label.CalcSize(content).ToRectSize().Position(buttonRect, Location.CENTER, 0.5f, 1);
                            GUI.Label(labelRect, content);
                        }
                        GUI.enabled = Utils.gameController.IsCurrent;
                    }

                    GUILayout.FlexibleSpace();

                    // End round button.
                    if (GUILayout.Button("End round", GUILayout.ExpandHeight(true)))
                    {
                        Utils.gameController.EndRound();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();

        // Minimap.
        GUI.DrawTexture(new Rect(Screen.width - MinimapSize - 10, 10, MinimapSize, MinimapSize), minimapTexture);
    }

    public void DrawEnd()
    {
        GUILayout.BeginArea(RectEx.ScreenRect);
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(Screen.width / 10);
                GUILayout.BeginVertical();
                {
                    GUILayout.FlexibleSpace();
                    
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("<size=80>You " + (Utils.gameController.IsWinner ? "won" : "lost") + "!!!</size>", GUILayout.Height(Screen.height / 5));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("<size=40>Main menu</size>", GUILayout.ExpandWidth(true), GUILayout.Height(Screen.height / 5)))
                    {
                        Application.LoadLevel("Menu");
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
                GUILayout.Space(Screen.width / 10);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }
}

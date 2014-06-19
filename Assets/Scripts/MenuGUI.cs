using MaienM.UnityUtils.CoreEx;
using UnityEngine;

public class MenuGUI : MonoBehaviour 
{
    public void OnGUI()
    {
        if (GUI.Button(RectEx.ScreenRect.Scale(0.7f).Center(RectEx.ScreenRect), "<size=80>Start game</size>"))
        {
            Application.LoadLevel("Game");
        }
    }
}

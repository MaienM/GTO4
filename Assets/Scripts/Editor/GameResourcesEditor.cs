using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

/*
[CustomPropertyDrawer(typeof(GameResources))]
public class GameResourcesEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.objectReferenceValue == null)
        {
            property.objectReferenceValue = new GameResources();
        }
        property.objectReferenceValue = new GameResources();

        GameResources resources = (GameResources)property.objectReferenceValue;
        EditorGUILayout.LabelField(resources[GameResourceType.IRON].ToString());
        /*EditorGUILayout.BeginVertical();
        {
            foreach (GameResourceType type in Enum.GetValues(typeof(GameResourceType)).Cast<GameResourceType>())
            {
                EditorGUILayout.LabelField("Hello", "World");
                //EditorGUILayout.FloatField(type.ToString(), resources[type]);
                //EditorGUILayout.LabelField("Hello", "World");
            }
        }
        EditorGUILayout.EndVertical();
    }
}
*/

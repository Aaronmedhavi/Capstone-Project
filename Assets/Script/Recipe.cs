using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe")]
public class Recipe : ScriptableObject
{
    public enum ColorItems { red, yellow, green, blue, purple, orange, brown }
    [Serializable]
    public class ColorCount
    {
        public ColorItems color; // Replace with your actual enum or class
        public int count;
    }
    [Serializable]
    public class ColorRecipe
    {
        public List<ColorCount> colors;
        public ColorItems result;
    }

    public List<ColorRecipe> recipes;
    public static Recipe.ColorItems GetRandomColor()
    {
        int length = Enum.GetValues(typeof(Recipe.ColorItems)).Length;
        return (Recipe.ColorItems)UnityEngine.Random.Range(0, length);
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Recipe.ColorCount))]
public class ColorCountDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Split the position into two parts for the tuple-like display
        float width = position.width / 2;

        Rect colorRect = new Rect(position.x, position.y, width - 5, position.height);
        Rect countRect = new Rect(position.x + width + 5, position.y, width - 5, position.height);

        // Draw fields
        SerializedProperty colorProp = property.FindPropertyRelative("color");
        SerializedProperty countProp = property.FindPropertyRelative("count");

        EditorGUI.PropertyField(colorRect, colorProp, GUIContent.none);
        EditorGUI.PropertyField(countRect, countProp, GUIContent.none);
    }
}
#endif

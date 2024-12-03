using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorInfo")]
public class ColorInfo : ScriptableObject
{
    [Serializable]
    public struct Info
    {
        public Recipe.ColorItems color;
        public List<PlayerData> projectiles;
    }
    [ArrayElementTitle("color")]
    [SerializeField] private List<Info> combos = new();
    public Info GetColor(Recipe.ColorItems color) => combos.Find(x => x.color == color);
}

public class ArrayElementTitleAttribute : PropertyAttribute
{
    public string Varname;
    public ArrayElementTitleAttribute(string ElementTitleVar)
    {
        Varname = ElementTitleVar;
    }
}
[CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
public class ArrayElementTitleDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                    GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    protected virtual ArrayElementTitleAttribute Atribute
    {
        get { return (ArrayElementTitleAttribute)attribute; }
    }
    SerializedProperty TitleNameProp;
    public override void OnGUI(Rect position,
                              SerializedProperty property,
                              GUIContent label)
    {
        string FullPathName = property.propertyPath + "." + Atribute.Varname;
        TitleNameProp = property.serializedObject.FindProperty(FullPathName);
        string newlabel = GetTitle();
        newlabel = newlabel[..1].ToUpper() + newlabel[1..];
        EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
    }
    private string GetTitle()
    {
        return TitleNameProp.enumNames[TitleNameProp.enumValueIndex];
    }
}
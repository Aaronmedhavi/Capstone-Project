using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorInfo")]
public class ColorInfo : ScriptableObject
{
    [Serializable]
    public struct ComboInfo
    {
        public Recipe.ColorItems color;
        public List<ProjectileCombo> projectiles;
    }
    [ArrayElementTitle("color")]
    [SerializeField] private List<ComboInfo> combos = new();

    public ComboInfo GetColor(Recipe.ColorItems color) => combos.Find(x => x.color == color);
#if UNITY_EDITOR
    [Header("Easy Assign (CUMAN DI UNITY EDITOR KOK)")]
    public List<ProjectileCombo> SetAll;
    public bool TurnOn;
    private void OnValidate()
    {
        if (TurnOn)
        {
            foreach (var combo in combos)
            {
                int count = SetAll.Count;
                for (int i = 0; i < count; i++)
                {
                    if (combo.projectiles.Count <= i) combo.projectiles.Add(new());
                    combo.projectiles[i] = SetAll[i];
                }
            }
        }
    }
#endif
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
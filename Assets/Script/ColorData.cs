using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Data")]
public class ColorData : ScriptableObject 
{
    [Serializable]
    public class Data
    {
        public Recipe.ColorItems type;
        public Color color;
    }
    public List<Data> datas = new ();

    public Color GetColor(Recipe.ColorItems type)
    {
        var data = datas.Find(x => x.type == type);
        return data != null ? data.color : Color.white;
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ColorData.Data))]
public class ColorDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float width = position.width / 2;

        Rect colorRect = new Rect(position.x, position.y, width - 5, position.height);
        Rect countRect = new Rect(position.x + width + 5, position.y, width - 5, position.height);

        SerializedProperty typeProp = property.FindPropertyRelative("type");
        SerializedProperty colorProp = property.FindPropertyRelative("color");

        EditorGUI.PropertyField(colorRect, typeProp, GUIContent.none);
        EditorGUI.PropertyField(countRect, colorProp, GUIContent.none);
    }
}
#endif

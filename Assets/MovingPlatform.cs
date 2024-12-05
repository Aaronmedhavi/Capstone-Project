using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float Platform_speed;
    [SerializeField] private float waitTime = 1f;
    [Header("SHIFT + LEFT CLICK AJA BWT BIKIN VECTORNYA YA\nInitial_Position udah dimasukkin jg")]
    public List<Vector2> waypoints = new();
    int pos = 0, Count;

    int currentpos => (pos >= Count / 2 ? Count - (pos + 1): pos);
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private void Start()
    {
        waypoints.Insert(0, transform.position);
        Count = waypoints.Count * 2;
    }
    private void Update()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
            }
            return;
        }
        Debug.Log(currentpos);
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentpos], Platform_speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints[currentpos]) <= 0.05f) ChangePos();
    }
    public void ChangePos()
    {
        pos = (pos + 1)%Count;
        isWaiting = true;
        waitTimer = waitTime;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    private void OnSceneGUI()
    {
        MovingPlatform platform = (MovingPlatform)target;

        // Handle Shift + Click to add waypoints
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0 && e.shift) // Left-click + Shift
        {
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            platform.waypoints.Add(mousePos);
            e.Use();
            EditorUtility.SetDirty(platform);
        }

        // Draw existing waypoints as handles
        for (int i = 0; i < platform.waypoints.Count; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector2 newPoint = Handles.PositionHandle(platform.waypoints[i], Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(platform, "Move Waypoint");
                platform.waypoints[i] = newPoint;
                EditorUtility.SetDirty(platform);
            }
        }
    }
    // Add an icon to the GameObject in the Scene view
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawWaypointIcons(MovingPlatform platform, GizmoType gizmoType)
    {
        Gizmos.color = Color.yellow; // Set the gizmo color
        foreach (Vector2 waypoint in platform.waypoints)
        {
            Gizmos.DrawSphere((Vector3)waypoint, 0.2f); // Use Unity's built-in icon
        }
    }
}
#endif

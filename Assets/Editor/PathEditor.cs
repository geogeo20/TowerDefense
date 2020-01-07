using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    
    /// <summary>
    /// Script to help me generate a bezier curve that enemies would follow
    /// </summary>
    
    private PathCreator creator;
    private Path path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        EditorGUI.BeginChangeCheck();
        
        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreatePath();
            path = creator.path;

        }
        
        
        if (GUILayout.Button("Toggle Close"))
        {
            Undo.RecordObject(creator, "Toogle Close");
            path.ToogleClosed();
            
        }

        if (GUILayout.Button("Show points"))
        {
            Debug.Log(path.NumPoints);
            
            for (int i = 0; i < path.NumPoints; i++)
            {
                Debug.Log(path[i]);
            }
        }

        bool autoSetControl = GUILayout.Toggle(path.SetControl, "Auto set controls");
        if (autoSetControl != path.SetControl)
        {
            Undo.RecordObject(creator, "Toogle Auto set");
            path.SetControl = autoSetControl;
            
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
        
    }

    private void OnSceneViewGUI(SceneView sv)
    {
        
        Draw();
        Input();
    }
    

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift )
        {
            Undo.RecordObject(creator, "Add segment");
            path.AddSegment(mousePos);
        }
        
    }
    
    void Draw()
    {
        float size;
        
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] points = path.GetPointsSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
        }
        
        
        for (int i = 0; i < path.NumPoints; i++)
        {
            if (i % 3 == 0)
            {
                Handles.color = Color.red;
                size = .15f;
            }
            else
            {
                Handles.color = Color.white;
                size = .1f;
            }
            
                
    
            Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, size, Vector2.zero, Handles.CylinderHandleCap);
            
            if (path[i] != newPos)
            {
                Undo.RecordObject(creator, "Move Point");
                path.MovePoint(i, newPos);
            }
        }
    }
    
    
    private void OnEnable()
    {
        creator = (PathCreator) target;
        if (creator.path == null)
        {
            creator.CreatePath();
            Debug.Log("Path Created");
        }
        path = creator.path;
        
        Debug.Log("OnEnable");
        SceneView.duringSceneGui += OnSceneViewGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneViewGUI;
        Debug.Log("OnDisable");
    }
}

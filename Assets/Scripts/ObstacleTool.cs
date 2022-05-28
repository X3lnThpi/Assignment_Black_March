using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObstacleTool : EditorWindow
{
    [MenuItem("Window / Obstacle Generator")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObstacleTool));
    }
    private void OnGUI()
    {
        
    }
}

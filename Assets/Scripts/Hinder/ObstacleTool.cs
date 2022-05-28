using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ObstacleTool : EditorWindow
{
    static int columns = 10;
    static int rows = 10;
    RectGridCell c;
    public RectGridViz vizCell;
    GameObject objx;

    [MenuItem("Window / Obstacle Generator")]

    private  void OnGUI()
    {
        GUILayout.Label("Obstacle Generator");
        for (int i = columns - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < rows; j++)
            {
                if (GUILayout.Button(j.ToString() + "," + i.ToString()))
                {
                    //ObstacleTool.instance.GenerateObstacle(j, i);
                    ObstacleManager.obstacle.GenerateSpheres(i, j);


                }
            }
            GUILayout.EndHorizontal();
        }

    }
}

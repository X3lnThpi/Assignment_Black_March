using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager obstacle;
    private Obstacles obs;
    
    public void GenerateSpheres(int row, int column)
    {
        Instantiate(obs.obstacle, new Vector3( (float) row, (float) column, 0f), Quaternion.identity);
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Obstacles", menuName ="Obstacles_Data")]
public class Obstacles : ScriptableObject
{
    public string name;
    public GameObject obstacle;
}

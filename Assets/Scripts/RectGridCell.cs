using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectGridCell : Node<Vector2Int>
{
    public bool isWalkable
    {
        get;
        set;
    }

    private RectGridViz rectGridViz;

    public RectGridCell(RectGridViz gridMap, Vector2Int value)
        :base(value)
    {
        rectGridViz = gridMap;
        isWalkable = true;
    }
    public override List<Node<Vector2Int>> GetNeighbours()
    {
        return rectGridViz.GetNeighbourCells(this);
        //throw new System.NotImplementedException();
    }
}

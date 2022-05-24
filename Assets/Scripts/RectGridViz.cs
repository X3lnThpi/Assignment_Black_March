using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectGridViz : MonoBehaviour
{
    public int row, column;

    [SerializeField]
    GameObject RectGridCell_prefab;

    GameObject[,] rectGridCellgameObjects;

    protected Vector2Int[,] indices;
    public float offsetX = 2.0f, offsetY = 2.0f;

    //2D array of RectGridCell
    protected RectGridCell[,] rectGridCell;

    protected void Construct(int row, int column)
    {
        this.row = row; 
        this.column = column;

        indices = new Vector2Int[row, column];
        rectGridCellgameObjects = new GameObject[row, column];
        rectGridCell = new RectGridCell[row, column];

       // RaycastHit hit;
        for (int i = 0; i < row; ++i)
        {
            for(int j = 0; j < column; ++j)
            {
                indices[i, j] = new Vector2Int(i, j);
                rectGridCellgameObjects[i, j] = Instantiate(RectGridCell_prefab, new Vector3(i + offsetX, j + offsetY, 0.0f), Quaternion.identity);

                rectGridCellgameObjects[i, j].transform.SetParent(transform);

                //Set proper names for generated cubes
                rectGridCellgameObjects[i, j].name = "Cube" + i + " " + j;

                //Create rectGridCell
                rectGridCell[i, j] = new RectGridCell(this, indices[i, j]);

                //set a reference to RectGrid_viz
                RectGridCellViz rectGridCellViz = rectGridCellgameObjects[i, j].GetComponent<RectGridCellViz>();
                if(rectGridCellViz != null)
                {
                    rectGridCellViz.rectGridCell = rectGridCell[i, j];
                }
            }
        }
        
    }

    void ResetCamera()
    {
        Camera.main.orthographicSize = column / 2.0f + 1.0f;
        Camera.main.transform.position = new Vector3(row / 2.0f - 0.5f, column / 2.0f - 0.5f, -100f);
    }

    public List<Node<Vector2Int>> GetNeighbourCells(Node<Vector2Int> loc)
    {
        List<Node<Vector2Int>> neighbours = new List<Node<Vector2Int>>();

        int x = loc.value.x;
        int y = loc.value.y;

        //check up
        if (y < row - 1)
        {
            int i = x;
            int j = y + 1;

            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }

        //check top right
        if(y < row - 1 && x < column - 1)
        {
            int i = x + 1;
            int j = y + 1;
            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }

        //check right
        if(x < column - 1)
        {
            int i = x + 1;
            int j = y;
            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }

        //Check down-right
        if (x < column - 1 && y > 0)
        {
            int i = x + 1;
            int j = y - 1;

            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }

        // Check down
        if (y > 0)
        {
            int i = x;
            int j = y - 1;

            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }

        // Check down-left
        if (y > 0 && x > 0)
        {
            int i = x - 1;
            int j = y - 1;

            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }

        // Check left
        if (x > 0)
        {
            int i = x - 1;
            int j = y;

            Vector2Int v = indices[i, j];

            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }
        // Check left-top
        if (x > 0 && y < row - 1)
        {
            int i = x - 1;
            int j = y + 1;

            if (rectGridCell[i, j].isWalkable)
            {
                neighbours.Add(rectGridCell[i, j]);
            }
        }


        return neighbours;

    }

    private void Start()
    {
        Construct(row, column);
        ResetCamera();
    }

    private void Update()
    {
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 300f))
        //{
        //    Debug.Log("hit");
        //}
    }
}

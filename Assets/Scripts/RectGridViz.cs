using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    //NPC
    public Transform destination;
    public NPCMovement npcMovement;
    public TMP_Text address, status;
    

    protected void Construct(int row, int column)
    {
        this.row = row; 
        this.column = column;

        indices = new Vector2Int[row, column];
        rectGridCellgameObjects = new GameObject[row, column];
        rectGridCell = new RectGridCell[row, column];

       
        for (int i = 0; i < row; ++i)
        {
            for(int j = 0; j < column; ++j)
            {
                indices[i, j] = new Vector2Int(i, j);
                rectGridCellgameObjects[i, j] = Instantiate(RectGridCell_prefab, new Vector3(i, j, 0.0f), Quaternion.identity);

                //Set parent for the grid cell to this transform.
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
                neighbours.Add(rectGridCell[i, j]); Debug.Log("Up Walkable");
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

    public void Display()
    {
        Vector2 rayPos = new Vector2(
         Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
         Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 200f);
        //Testing
        RaycastHit hitx;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitx))
        {
            Debug.Log("Got hit");
            GameObject objx = hitx.transform.gameObject;
            RectGridCellViz sc = objx.GetComponent<RectGridCellViz>();
            Debug.Log(objx.name);
            address.text = objx.name;
            if (sc.rectGridCell.isWalkable)
            {
                status.text = "walkable";
            }
            if (!sc.rectGridCell.isWalkable)
            {
                status.text = "not walkable";
            }
            //objx.GetComponent<Renderer>().material.color = Color.green;
            //Debug.Log("Walkable is " + sc.rectGridCell.isWalkable);
        }
        if (hit)
        {
            GameObject obj = hit.transform.gameObject;
            RectGridCellViz sc = obj.GetComponent<RectGridCellViz>();
            //Debug.Log("hit");
            //ToggleWalkable(sc);
        }
    }

    public void ToggleWalkable()
    {
        Vector2 rayPos = new Vector2(
        Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
        Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 200f);
        //Testing
        RaycastHit hitx;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitx))
        {
            //Debug.Log("Got hit");
            GameObject objx = hitx.transform.gameObject;
            RectGridCellViz sc = objx.GetComponent<RectGridCellViz>();
            //Debug.Log(objx.name);
            objx.GetComponent<Renderer>().material.color = Color.green;
            bool tset = sc.rectGridCell.isWalkable = false;
            //Debug.Log(tset);
        }
    }

    //Set Destination
    void RayCastAndSetDestination()
    {
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 200f);
        RaycastHit hitx;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitx))
        {
            GameObject obj = hitx.transform.gameObject;
            RectGridCellViz sc = obj.GetComponent<RectGridCellViz>();
            if(sc == null)
            {
                Debug.Log("sc == null");
                return;
            }
            Debug.Log("Rt Click registered");
            Vector3 pos = destination.position;
            pos.x = sc.rectGridCell.value.x;
            pos.y = sc.rectGridCell.value.y;
            destination.position = pos;
            Debug.Log("Walkable is " + sc.rectGridCell.isWalkable);
            //Set the destination to the NPC
            npcMovement.SetDestination(this, sc.rectGridCell);
        }
    }

    public void ToggleWalkable(RectGridCellViz sc)
    {
        Debug.Log("Changed");
    }

    public static float GetManhattanCost(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public static float GetEuclideanCost(Vector2Int a, Vector2Int b)
    {
        return GetCostBetweenTwoCells(a, b);
    }

    //Helper method that returns a RectGridCell based on row & Column values
    public RectGridCell GetRectGridCell(int x, int y)
    {
        if(x >= 0 && x < row && y >= 0 && y < column)
        {
            return rectGridCell[x, y];
        }
        return null;
    }

    public static float GetCostBetweenTwoCells(Vector2Int a, Vector2Int b)
    {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
    }

    private void Start()
    {
        Construct(row, column);
        ResetCamera();
    }

    private void Update()
    {
        Display();
        if (Input.GetMouseButtonDown(0))
        {
            ToggleWalkable();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RayCastAndSetDestination();
        }
    }
}

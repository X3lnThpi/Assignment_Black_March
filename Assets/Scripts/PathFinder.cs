using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFinder<T> 
{
    #region Delegates for cost calculatons
    //Delegate that defines signature for calculating the cost between two nodes
    public delegate float CostFunction(T a, T b);
    public CostFunction HeuristicCost { get; set; }
    public CostFunction NodeTraversalCost { get; set; }

    #endregion

    #region PathFinderNode
    // The pathfinder class equates to a node in a tree generated by the pathfinder, in its search for the most optimal path
    // This class encapsulates a Node and hold other attributes needed for search traversal
    // The pathfinder creates instances of this class at runtime, while doing search

    public class PathFinderNode : System.IComparable<PathFinderNode>
    {
        // Parent of this Node
        public PathFinderNode parent { get; set; }

        //Node this PathFinder is pointing to
        public Node<T> location { get; private set; }

        // The various costs
        public float FCost { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }

        // The COnstructor
        public PathFinderNode(Node<T> location, PathFinderNode parent, float gCost, float hCost)
        {
            this.location = location;
            this.parent = parent;
            HCost = hCost;
            SetGCost(gCost);
        }

        //set the GCost
        public void SetGCost(float c)
        {
            GCost = c;
            FCost = GCost + HCost;
        }

        public int CompareTo(PathFinderNode other)
        {
            if (other == null) return 1;
            return FCost.CompareTo(other.FCost);
        }

    }

    #endregion

    #region Properties

    //Property that holds the current status of the pathfinder, set to not-initiliazed by default
    //Private to ensure only this class can change and set the status
    public PathFinderStatus status
    {
        get;
        private set;
    } = PathFinderStatus.NOT_INITIALIZED;

    //Add properties for the start and goal nodes
    public Node<T> start { get; private set; }
    public Node<T> goal { get; private set; }

    //The property to access the currentNode that the pathfinder is now at
    public PathFinderNode currentNode { get; set; }

    #endregion

    #region Open and closed lists and associated functions

    // The open list for the path finder
    protected List<PathFinderNode> openList = new List<PathFinderNode>();

    //The closed list
    protected List<PathFinderNode> closedList = new List<PathFinderNode>();

    // A helper method to find the lest cost node from a list
    protected PathFinderNode GetLeastCostNode(List<PathFinderNode> myList)
    {
        int bestIndex = 0;
        float bestPriority = myList[0].FCost;
        for(int i = 1; i < myList.Count; i++)
        {
            if(bestPriority > myList[i].FCost)
            {
                bestPriority = myList[i].FCost;
                bestIndex = i;
            }
        }

        PathFinderNode n = myList[bestIndex];
        return n;
    }

    //Helper method to check if a value of T is in a list
    //If it is then return the index of the item where the value is, otherwise return -1
    protected int IsInList(List<PathFinderNode> myList, T cell)
    {
        for(int i = 0; i < myList.Count; ++i)
        {
            if(EqualityComparer<T>.Default.Equals(myList[i].location.value, cell))
            {
                return i;
            }
        }
        return -1;
    }

    #endregion

    #region Delegates for action callback

    //Callbacks to handle on changes to the internal values
    //these changes can be used by the game to display visually the changes to the cells and lists
    public delegate void DelegatePathFinderNode(PathFinderNode node);
    public DelegatePathFinderNode onChangeCurrentNode;
    public DelegatePathFinderNode onAddToOpenList;
    public DelegatePathFinderNode onAddToClosedList;
    public DelegatePathFinderNode onDestinationFound;

    public delegate void DelegateNoArgument();
    public DelegateNoArgument onStarted;
    public DelegateNoArgument onRunning;
    public DelegateNoArgument onFailure;
    public DelegateNoArgument onSucess;

    #endregion

    #region Actual path finding functions

    //Initialize the search
    //Search can only be initialized if the path finder is not already running
    //

    public bool Initialize(Node<T> start, Node<T> goal)
    {
        if(status == PathFinderStatus.RUNNING)
        {
            //Path finder is already is already in progress
            return false;
        }

        //Reset the Variable
        Reset();

        //Set the start and the goal nodes for this search
        this.start = start;
        this.goal = goal;

        //Calculate the H cost for the start
        float H = HeuristicCost(start.value, goal.value);

        //create a root node with its parents as null
        PathFinderNode root = new PathFinderNode(start, null, 0f, H);

        //Add this root node to our open list
        openList.Add(root);

        //set the current node to root node
        currentNode = root;

        //Invoke the delegates to inform the caller if the delegates are not null
        onChangeCurrentNode?.Invoke(currentNode);
        onStarted?.Invoke();

        //Set the status of the pathfinder to Running
        status = PathFinderStatus.RUNNING;

        return true;
    }

    //Stage2 Step until success or failure
    //Take as search step. the user must continue to call this method
    //until the status is either success or failure

    public PathFinderStatus Step()
    {
        //Add the current node to the closed list
        closedList.Add(currentNode);

        //Call the delegate to inform any subscribers
        onAddToClosedList?.Invoke(currentNode);

        if(openList.Count == 0)
        {
            //We have exhausted our search, No solution is found
            status = PathFinderStatus.FAILURE;
            onFailure?.Invoke();
            return status;
        }

        //Get the least cost element form the open list
        //this becomes our new current node
        currentNode = GetLeastCostNode(openList);

        //Call the delegate to inform any subscribers
        onChangeCurrentNode?.Invoke(currentNode);

        //Remove the node from the open list
        openList.Remove(currentNode);

        //check if the node contains the goal cell
        if(EqualityComparer<T>.Default.Equals(currentNode.location.value, goal.value))
        {
            status = PathFinderStatus.SUCCESS;
            onDestinationFound?.Invoke(currentNode);
            onSucess?.Invoke();
            return status;
        }

        //Find the neighbors
        List<Node<T>> neighbours = currentNode.location.GetNeighbours();

        //Traverse each of these neighbours for possible expansion
        foreach(Node<T> cell in neighbours)
        {
            AlgorithmSpecificImplementation(cell);
        }

        status = PathFinderStatus.RUNNING;
        onRunning?.Invoke();
        return status;
    }

    abstract protected void AlgorithmSpecificImplementation(Node<T> cell);

    //Reset the internal variable for a new search
    protected void Reset()
    {
        if(status == PathFinderStatus.RUNNING)
        {
            //cannot reset path finder. path finding in progress
            return;
        }

        currentNode = null;

        openList.Clear();
        closedList.Clear();

        status = PathFinderStatus.NOT_INITIALIZED;
    }

    #endregion

}

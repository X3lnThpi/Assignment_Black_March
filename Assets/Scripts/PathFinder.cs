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

    }

    #endregion
}
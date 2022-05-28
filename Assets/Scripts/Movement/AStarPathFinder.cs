using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder<T> : PathFinder<T> 
{

    protected override void AlgorithmSpecificImplementation(Node<T> cell)
    {
        //First check if the node is already in the closedList
        //if so then we do not need to continue search for this node
        if(IsInList(closedList, cell.value) == -1)
        {
            // The cell does not exist in the closed list
            //Calculate the cost of the node from its parent
            //G- is the cost pf the currentNode and we add the cost from currentNode to this cell.
            //We can implement a function to calculate the cost between two adjacent cell

            float G = currentNode.GCost + NodeTraversalCost(currentNode.location.value, cell.value);

            float H = HeuristicCost(cell.value, goal.value);

            //check if the cell is already there in the open list
            int idOList = IsInList(openList, cell.value);
            if(idOList == -1)
            {
                // The cell does not exist in the open list
                // We will add the cell to the open list

                PathFinderNode n = new PathFinderNode(cell, currentNode, G, H);
                openList.Add(n);
                onAddToOpenList?.Invoke(n);
            }
            else
            {
                //If the cell exists in the open-list then check if the G cost is less than the one already in the list
                float oldG = openList[idOList].GCost;
                if(G < oldG)
                {
                    //change the parent and update the cost to the new G
                    openList[idOList].parent = currentNode;
                    openList[idOList].SetGCost(G);
                    onAddToOpenList?.Invoke(openList[idOList]);
                }
            }
        }

       // throw new System.NotImplementedException();
    }

}

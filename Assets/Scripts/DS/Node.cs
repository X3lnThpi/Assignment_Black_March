using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathFinderStatus
{
    NOT_INITIALIZED,
    SUCCESS,
    FAILURE,
    RUNNING

} 
abstract public class Node<T> 
{
    public T value
    {
        get;
        private set;
    }

    //Base constructor of Node class
    public Node(T value)
    {
        this.value = value;
    }

    //Get neighbors for this Node
    abstract public List<Node<T>> GetNeighbours();
}

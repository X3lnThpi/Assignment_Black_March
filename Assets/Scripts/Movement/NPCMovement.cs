using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 1.0f;

    public Queue<Vector2> wayPoints = new Queue<Vector2>();

    PathFinder<Vector2Int> pathFinder = new AStarPathFinder<Vector2Int>();

    private void Start()
    {
        pathFinder.onSucess = OnSuccessPathFinding;
        pathFinder.onFailure = OnFailurePathFinding;
        pathFinder.HeuristicCost = RectGridViz.GetManhattanCost;
        pathFinder.NodeTraversalCost = RectGridViz.GetEuclideanCost;
        StartCoroutine(CoroutineMoveTo());
    }

    public void AddWayPoint(Vector2 pt)
    {
        wayPoints.Enqueue(pt);
    }

    public void SetDestination(RectGridViz map, RectGridCell destination)
    {
       // AddWayPoint(destination.value);
        if(pathFinder.status == PathFinderStatus.RUNNING)
        {
            Debug.Log("PathFinder already running, cannot set destination now");
            return;
        }
        //remove all way-points from the queue
        wayPoints.Clear();
        //new start location is previous destination
        RectGridCell start = map.GetRectGridCell((int)transform.position.x, (int)transform.position.y);
        if (start == null) return;
        pathFinder.Initialize(start, destination);
        StartCoroutine(CoroutineFindPathSteps());

    }

    public IEnumerator CoroutineFindPathSteps()
    {
        while(pathFinder.status == PathFinderStatus.RUNNING)
        {
            pathFinder.Step();
            yield return null;
        }
    }

    public IEnumerator CoroutineMoveTo()
    {
        while (true)
        {
            while(wayPoints.Count > 0)
            {
                yield return StartCoroutine(CoroutineMoveToPoint(wayPoints.Dequeue(), speed));
            }
            yield return null;
        }      
    }

    //Coroutines to move smoothly
    private IEnumerator CoroutineMoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = objectToMove.transform.position;
        
        while(elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    IEnumerator CoroutineMoveToPoint(Vector2 p, float speed)
    {
        Vector3 endP = new Vector3(p.x, p.y, transform.position.z);
        float duration = (transform.position - endP).magnitude / speed;
        yield return StartCoroutine(CoroutineMoveOverSeconds(transform.gameObject, endP, duration));
    }

    void OnSuccessPathFinding()
    {
        PathFinder<Vector2Int>.PathFinderNode node = pathFinder.currentNode;
        List<Vector2Int> reverseIndices = new List<Vector2Int>();
        while(node != null)
        {
            reverseIndices.Add(node.location.value);
            node = node.parent;
        }
        for(int i = reverseIndices.Count -1; i >= 0; i--)
        {
            AddWayPoint(new Vector2(reverseIndices[i].x, reverseIndices[i].y));
        }
    }

    void OnFailurePathFinding()
    {
        Debug.Log("Error: Cannot find Path");
    }
}

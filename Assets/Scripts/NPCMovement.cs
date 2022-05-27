using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 1.0f;

    public Queue<Vector2> wayPoints = new Queue<Vector2>();

    //PathFinder<Vector2Int> pathFinder = new AStar

    private void Start()
    {
        StartCoroutine(CoroutineMoveTo());
    }

    public void AddWayPoint(Vector2 pt)
    {
        wayPoints.Enqueue(pt);
    }

    public void SetDestination(RectGridViz map, RectGridCell destination)
    {
        AddWayPoint(destination.value);
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
}

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    Path path;
    AIPath mover;
    Seeker seeker;
    GameObject target = null;

    EnemyState state;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        //target = targetPosition.gameObject;
        mover = GetComponent<AIPath>();
        // Get a reference to the Seeker component we added earlier
        seeker = GetComponent<Seeker>();

        state = EnemyState.chasing;
        
        // Start to calculate a new path to the targetPosition object, return the result to the OnPathComplete method.
        // Path requests are asynchronous, so when the OnPathComplete method is called depends on how long it
        // takes to calculate the path. Usually it is called the next frame.
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            
        }
        else
        {
            Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.patrolling:
                //Play animation of the player

                if (path == null)
                {
                    seeker.StartPath(transform.position, target.transform.position, OnPathComplete);

                    return;
                }

                //reachedEndOfPath = false;
                // The distance to the next waypoint in the path
                float distanceToWaypoint;
                /*
                while (true)
                {
                    
                    //This is where patrolling logic will be
                    
                    distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                    if (distanceToWaypoint < nextWaypointDistance)
                    {

                        // Check if there is another waypoint or if we have reached the end of the path
                        if (currentWaypoint + 1 < path.vectorPath.Count)
                        {
                            currentWaypoint++;
                        }
                        else
                        {
                            // Set a status variable to indicate that the agent has reached the end of the path.
                            // You can use this to trigger some special code if your game requires that.

                            reachedEndOfPath = true;
                            IncreaseWaypointIndex();
                            seeker.StartPath(transform.position, waypoints[waypointIndex].position, OnPathComplete);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    
                }*/

                //Add view cone logic for enemy here, if they spot the player they should chase, if not fuck around I guess? Or walk in general direction of player?

                LayerMask mask = LayerMask.GetMask("Player");
                Collider[] playerColliders = Physics.OverlapSphere(transform.position, 6, mask);

                if (playerColliders.Length > 0)
                {
                    //Detection wait time
                    StartCoroutine(SwitchStates(1f, "DogDetect", EnemyState.chasing));
                }
                break;
            case EnemyState.chasing:

                mask = LayerMask.GetMask("Player");

                //Need some sort of Line of sight to player logic
                if (target != null)
                {
                    seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
                }
                else
                {
                    //Set status to patrolling
                    SwitchStates(1f, "Wait", EnemyState.patrolling);
                }
                break;
            case EnemyState.busy:
                return;
            default:
                break;
        }
    }

    IEnumerator SwitchStates(float waitTime, string waitAnimation, EnemyState newState)
    {
        mover.canMove = false;
        state = EnemyState.busy;
        //Play wait animation, guy waving his hands or smth.
        yield return new WaitForSeconds(waitTime);
        state = newState;
        mover.canMove = true;
    }


}

public enum EnemyState
{
    patrolling,
    chasing,
    busy
}

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHittable
{
    public void Hit()
    {
        Debug.Log("Ouch!!!");
        GetComponent<EnemyPathfinding>().enabled = false;
        GetComponent<AIPath>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

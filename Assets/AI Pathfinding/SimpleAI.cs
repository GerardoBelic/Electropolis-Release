using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Transform target;

    [SerializeField] private bool stop = false;

    // Update is called once per frame
    void Update()
    {
        if (!stop)
            agent.SetDestination(target.position);
        //print(agent.remainingDistance);
        //print(agent.isOnNavMesh); /// This property tell us if the agent is on a navmesh and can move
    }
}

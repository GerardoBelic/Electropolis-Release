using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Rabbit_Hole_Teleporter : MonoBehaviour
{

    /// When a NPC is going to exit a building, it will spawn in one of this random spots
    [SerializeField] private List<Transform> spawns = new List<Transform>();

    /// Event to notify (a building/NPC) that he entered a teleporter trigger
    [SerializeField] private UnityEvent teleporter_trigger = new UnityEvent();

    public Transform get_random_spawn()
    {
        System.Random rnd = new System.Random();
        var random_index = rnd.Next(spawns.Count);

        return spawns[random_index];
    }

    void Awake()
    {

        /// Check if the spawns are assigned to the list
        if (spawns.Count == 0)
        {
            Debug.LogError("No spawn(s) set");
        }

        /// Check if there is a collider mark as trigger
        Collider col = gameObject.GetComponent<Collider>();

        if (col == null)
        {
            Debug.LogError("Missing collider");
        }

        if (!col.isTrigger)
        {
            Debug.LogError("The collider is not a trigger");
        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        /// TODO: we must ask NPC if he wants to enter building (to teleport him) or is just leaving the building
        /// (in this case we do nothing)
        teleporter_trigger.Invoke();
    }

}

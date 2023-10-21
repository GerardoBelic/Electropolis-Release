using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Drive_Teleporter : MonoBehaviour
{

    /// Spawn point when the NPC starts to drive
    [SerializeField] private Transform spawn_point;

    /// Since there are two teleporters/hitboxes (one in the sidewalk and another in the road), we must keep a reference
    /// of the other teleporter
    [SerializeField] private Walk_Teleporter other_teleporter;

    /// Event to notify (a NPC) that he entered a teleporter trigger
    [SerializeField] private UnityEvent teleporter_trigger = new UnityEvent();

    void Awake()
    {
        /// Check the spawn point
        if (spawn_point == null)
        {
            Debug.LogError("Spawn point not set");
        }

        /// Check if we setted the walk teleporter
        if (other_teleporter == null)
        {
            Debug.LogError("Other walk/drive teleporter not set");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        /// TODO: we must ask NPC if he wants to teleport to walk/drive mode or he is just exiting current mode
        teleporter_trigger.Invoke();
    }


}

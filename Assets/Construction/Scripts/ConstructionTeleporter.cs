using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTeleporter : MonoBehaviour
{

    [SerializeField] private Transform building_teleport;

    void Awake()
    {
        /// Check that the building teleport has been assigned
        if (building_teleport == null)
        {
            //Debug.LogError("Building teleport not assigned");
            building_teleport = gameObject.transform.Find("Entrance");

            if (building_teleport == null)
            {
                Debug.LogError("Building teleport not assigned");
            }
        }
    }

    /// 1- If we go out of a building, we teleport to the building entrance point
    /// 2- If we go to a building, we must know where is the entrance
    public Transform get_building_entrance_position()
    {
        return building_teleport;
    }

}

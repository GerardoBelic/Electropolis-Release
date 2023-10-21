using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTeleporter : MonoBehaviour
{
    
    [SerializeField] private List<Transform> sidewalk_teleports;
    [SerializeField] private Transform road_teleport;

    void Awake()
    {
        /// Check the sidewalk and road teleports
        if (sidewalk_teleports == null)
        {
            Debug.LogError("Sidewalk(s) teleport(s) not assigned");
        }

        if (road_teleport == null)
        {
            Debug.LogError("Road teleport not assigned");
        }
    }

    /// 1- If we go from a building to a road, we must know what is the closest teleport from their origin
    /// 2- If we go from a road to a building, we must know what is the closest teleport to the destination
    public Vector3 closest_sidewalk_position(Vector3 position)
    {
        Transform closest_sidewalk = null;
        float min_distance_sq = Mathf.Infinity;

        foreach (Transform t in sidewalk_teleports)
        {
            float distance_sq = (position - t.position).sqrMagnitude;

            if (distance_sq < min_distance_sq)
            {
                closest_sidewalk = t;
                min_distance_sq = distance_sq;
            }
        }

        return closest_sidewalk.position;
    }

    public Transform get_road_teleport()
    {
        return road_teleport;
    }

}

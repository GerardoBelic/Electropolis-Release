using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneBuilding : MonoBehaviour, ISurroundRoads, IOccupiedSpace
{
    
    private Building standalone_building;

    /*public StandaloneBuilding(Building building)
    {
        standalone_building = building;
    }*/

    void Start()
    {
        standalone_building = gameObject.GetComponent<Building>();

        if (standalone_building == null)
        {
            Debug.LogError("Building not found");
        }
    }

    public Building get_building()
    {
        return standalone_building;
    }

    #region Surround roads

    private List<Vector3Int> road_tiles;

    public List<Vector3Int> get_surround_roads()
    {
        return road_tiles;
    }

    public void set_surround_roads(List<Vector3Int> roads)
    {
        road_tiles = roads;
    }

    #endregion

    #region Space occupied by building

    private List<Vector3Int> occupied_space;

    public List<Vector3Int> get_occupied_space()
    {
        return occupied_space;
    }

    public void set_occupied_space(List<Vector3Int> space)
    {
        occupied_space = space;
    }

    #endregion

}

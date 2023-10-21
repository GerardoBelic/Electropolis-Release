using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingZone : MonoBehaviour, ISurroundRoads, IOccupiedSpace
{
    private List<Building> zone_buildings;

    public BuildingZone(List<Building> buildings)
    {
        zone_buildings = buildings;
    }

    public List<Building> get_buildings()
    {
        return zone_buildings;
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

    #region Space occupied by zone

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

    #region Paint brush info

    [Range (0, 2)]
    [SerializeField] private int tile_index = 0;

    public int get_tile_index()
    {
        return tile_index;
    }

    #endregion
}

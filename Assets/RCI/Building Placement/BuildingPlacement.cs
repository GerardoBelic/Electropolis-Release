using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Building_Placement_Mode
{
    None,
    Standalone,
    Zone
}

public class BuildingPlacement : MonoBehaviour, ISurroundRoads, IOccupiedSpace
{
    
    [SerializeField] private Building_Placement_Mode building_placement_mode = Building_Placement_Mode.None;

    [SerializeField] private StandaloneBuilding standalone_building = null;

    [SerializeField] private BuildingZone building_zone = null;

    /*public BuildingPlacement(StandaloneBuilding _standalone_building)
    {
        standalone_building = _standalone_building;
        building_placement_mode = Building_Placement_Mode.Standalone;
    }

    public BuildingPlacement(BuildingZone _building_zone)
    {
        building_zone = _building_zone;
        building_placement_mode = Building_Placement_Mode.Zone;
    }*/

    void Start()
    {
        /*standalone_building = gameObject.GetComponent<StandaloneBuilding>();
        building_zone = gameObject.GetComponent<BuildingZone>();

        if (standalone_building != null)
        {
            building_placement_mode = Building_Placement_Mode.Standalone;
        }
        else if (building_zone != null)
        {
            building_placement_mode = Building_Placement_Mode.Zone;
        }
        else
        {
            Debug.LogError("No standalone building or building zone found");
        }*/
    }

    public Building_Placement_Mode get_building_placement_mode()
    {
        return building_placement_mode;
    }

    public int get_tile_index()
    {
        if (building_placement_mode == Building_Placement_Mode.Zone)
        {
            return building_zone.get_tile_index();
        }

        return -1;
    }

    #region Surround roads

    public List<Vector3Int> get_surround_roads()
    {
        if (building_placement_mode == Building_Placement_Mode.Standalone)
        {
            return standalone_building.get_surround_roads();
        }
        else
        {
            return building_zone.get_surround_roads();
        }
    }

    public void set_surround_roads(List<Vector3Int> roads)
    {
        if (building_placement_mode == Building_Placement_Mode.Standalone)
        {
            standalone_building.set_surround_roads(roads);
        }
        else
        {
            building_zone.set_surround_roads(roads);
        }
    }

    #endregion

    #region Space occupied by building placement

    public List<Vector3Int> get_occupied_space()
    {
        if (building_placement_mode == Building_Placement_Mode.Standalone)
        {
            return standalone_building.get_occupied_space();
        }
        else
        {
            return building_zone.get_occupied_space();
        }
    }

    public void set_occupied_space(List<Vector3Int> space)
    {
        if (building_placement_mode == Building_Placement_Mode.Standalone)
        {
            standalone_building.set_occupied_space(space);
        }
        else
        {
            building_zone.set_occupied_space(space);
        }
    }

    #endregion

}

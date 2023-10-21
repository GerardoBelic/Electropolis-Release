using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    
    internal int id_building;

    [HideInInspector] public GameObject building_gameobject;
    internal BuildingPlacement building_placement;

    internal void initialize_building_placement()
    {
        building_gameobject = gameObject;

        building_placement = gameObject.GetComponent<BuildingPlacement>();
        if (building_placement == null)
        {
            Debug.LogError("Building doesn't have a BuildingPlacement component");
        }
    }

    internal ConstructionTeleporter construction_teleporter;
    internal Transform construction_entrance;

    internal void initialize_building_entrance()
    {
        construction_teleporter = gameObject.GetComponent<ConstructionTeleporter>();
        if (construction_teleporter == null)
        {
            Debug.LogError("Building doesn't have a ConstructionTeleporter component");
        }

        construction_entrance = construction_teleporter.get_building_entrance_position();
    }

    void Awake()
    {
        initialize_building_placement();
        initialize_building_entrance();

        id_building = get_unique_building_id();
    }

    #region Get unique ID for building

    internal static int building_id_counter = 0;

    internal static int get_unique_building_id()
    {
        return building_id_counter++;
    }

    #endregion

}

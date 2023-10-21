using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionInfo : MonoBehaviour
{

    public enum Construction_Classification
    {
        Residence,
        Commerce,
        Industry,
        Service,
        Other
    }

    public enum Construction_Placement_Type
    {
        Building,
        Brush
    }

    public enum Construction_Lock_Status
    {
        Unlocked,
        Locked,
    }

    public string construction_name = "construction name";
    public List<string> tags = new List<string>{"default tag"};
    [HideInInspector] public Construction_Classification construction_classification;
    [HideInInspector] public Construction_Placement_Type construction_placement_type;
    //public GameObject prefab;
    [HideInInspector] public Construction_Lock_Status construction_lock_status;
    [HideInInspector] public Sprite construction_render;

    public void preinitialize_construction_info()
    {
        construction_name = gameObject.name;

        BuildingPlacement building_placement = gameObject.GetComponent<BuildingPlacement>();

        if (building_placement.get_building_placement_mode() == Building_Placement_Mode.Zone)
        {
            if (construction_name.Contains("Residence"))
            {
                construction_classification = Construction_Classification.Residence;
            }
            else if (construction_name.Contains("Commerce"))
            {
                construction_classification = Construction_Classification.Commerce;
            }
            else if (construction_name.Contains("Industry"))
            {
                construction_classification = Construction_Classification.Industry;
            }
            else
            {
                construction_classification = Construction_Classification.Other;
            }

            return;
        }

        Building building = gameObject.GetComponent<Building>();

        if (building is ResidenceBuilding)
        {
            construction_classification = Construction_Classification.Residence;
        }
        else if (building is CommerceBuilding || building is OfficeBuilding)
        {
            construction_classification = Construction_Classification.Commerce;
        }
        else if (building is IndustrialBuilding)
        {
            construction_classification = Construction_Classification.Industry;
        }
        else
        {
            construction_classification = Construction_Classification.Other;
        }
    }

    /// TODO: import save file to read if we have unlocked some of the constructions
    void import_construction_status()
    {

    }

    /// TODO: to not render a construction over and over, read a render if it exist
    void import_construction_render()
    {

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

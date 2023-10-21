using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingDictionary : MonoBehaviour
{

    #region Building dictionary with tile position as key and building placement as value
    
    private Dictionary<Vector3Int, BuildingPlacement> building_dictionary = new Dictionary<Vector3Int, BuildingPlacement>();

    /// With a tile position, try to get a building if anything is placed there
    public BuildingPlacement get_building_placement(Vector3Int tile_position)
    {
        BuildingPlacement building_placement;

        /// If in the tile there is a building, return it
        if (building_dictionary.TryGetValue(tile_position, out building_placement))
        {
            return building_placement;
        }

        /// Else return that there is nothing in the tile
        return null;
    }

    public void add_building_placement(BuildingPlacement building_placement)
    {
        List<Vector3Int> occupied_space = building_placement.get_occupied_space();

        foreach (Vector3Int tile in occupied_space)
        {
            building_dictionary[tile] = building_placement;
        }
    }

    #endregion

    #region Building dictionary with ID as key and a building as value



    #endregion

    #region Plain building list

    private List<Building> buildings = new List<Building>();

    public void add_building(Building building)
    {
        buildings.Add(building);
    }

    private static System.Random random = new System.Random();

    public Transform get_random_building_position()
    {
        int random_index = random.Next(buildings.Count);

        Building building = buildings[random_index];

        ConstructionTeleporter construction_teleporter = building.building_gameobject.GetComponent<ConstructionTeleporter>();

        return construction_teleporter.get_building_entrance_position();
    }

    #endregion

    #region Population count

    private static float wait_time_until_population_recount = 10.0f;

    private int population_count = 0;

    public int get_population_count()
    {
        return population_count;
    }

    private Coroutine population_recount_coroutine = null;
    private IEnumerator population_recount()
    {
        while (true)
        {
            int new_population_count = 0;

            foreach (ResidenceBuilding building in buildings.OfType<ResidenceBuilding>())
            {
                new_population_count += building.get_total_home_slots();
            }

            population_count = new_population_count;

            yield return new WaitForSeconds(wait_time_until_population_recount);

            //print(population_count);
        }
    }

    #endregion

    #region Money and job count

    private static float wait_time_until_job_recount = 10.0f;
    private static float wait_time_until_money_generation_recount = 10.0f;

    private int money_generation_per_hour = 0;
    private int job_count = 0;

    public int get_money_generation_per_hour()
    {
        return money_generation_per_hour;
    }

    public int get_job_count()
    {
        return job_count;
    }

    private Coroutine money_recount_coroutine = null;
    private IEnumerator money_recount()
    {
        while (true)
        {
            int new_money_count = 0;

            foreach (CommerceBuilding building in buildings.OfType<CommerceBuilding>())
            {
                new_money_count += building.get_money_geneated_per_hour_per_job() * building.get_total_job_slots();
            }

            foreach (OfficeBuilding building in buildings.OfType<OfficeBuilding>())
            {
                new_money_count += building.get_money_geneated_per_hour_per_job() * building.get_total_job_slots();
            }

            foreach (IndustrialBuilding building in buildings.OfType<IndustrialBuilding>())
            {
                new_money_count += building.get_money_geneated_per_hour_per_job() * building.get_total_job_slots();
            }

            money_generation_per_hour = new_money_count;

            yield return new WaitForSeconds(wait_time_until_money_generation_recount);

            //print(money_generation_per_hour);
        }
    }

    private Coroutine job_recount_coroutine = null;
    private IEnumerator job_recount()
    {
        while (true)
        {
            int new_job_count = 0;

            foreach (CommerceBuilding building in buildings.OfType<CommerceBuilding>())
            {
                new_job_count += building.get_total_job_slots();
            }

            foreach (OfficeBuilding building in buildings.OfType<OfficeBuilding>())
            {
                new_job_count += building.get_total_job_slots();
            }

            foreach (IndustrialBuilding building in buildings.OfType<IndustrialBuilding>())
            {
                new_job_count += building.get_total_job_slots();
            }

            job_count = new_job_count;

            yield return new WaitForSeconds(wait_time_until_job_recount);

            //print(job_count);
        }
    }

    #endregion

    #region Unity methods

    public GameObject building_dictionary_gameobject;

    void Awake()
    {
        building_dictionary_gameobject = gameObject;
    }

    void Start()
    {
        population_recount_coroutine = StartCoroutine(population_recount());
        money_recount_coroutine = StartCoroutine(money_recount());
        job_recount_coroutine = StartCoroutine(job_recount());
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System;
using System.IO;

[Serializable]
public class Ser_Building_Placement
{
    public string building_prefab_name;
    public Vector3 building_global_position;
    public Vector3Int building_size;
    public Vector3Int building_start_position;
}

[Serializable]
public class Ser_Buildings
{
    public List<Ser_Building_Placement> buildings = new List<Ser_Building_Placement>();
}

[Serializable]
public class Ser_Road_Positions
{
    public List<Vector3Int> road_tiles = new List<Vector3Int>();
}

[Serializable]
public class Ser_Tilemap_Markzone
{
    public List<Vector3Int> tiles_0 = new List<Vector3Int>();
    public List<Vector3Int> tiles_1 = new List<Vector3Int>();
    public List<Vector3Int> tiles_2 = new List<Vector3Int>();
}

[Serializable]
public class Ser_Game_State
{
    public Ser_Buildings buildings;
    public Ser_Road_Positions road_positions;
    public Ser_Tilemap_Markzone tilemap_markzone;
}

public class LoaderSaverManager : MonoBehaviour
{

    [SerializeField] private GameObject building_dictionary;
    [SerializeField] private RoadNetwork road_network;
    [SerializeField] private TilemapController markzone_tilemap_controller;

    public void save_state()
    {
        /// Buildings
        Ser_Buildings buildings = new Ser_Buildings();
        
        foreach (Transform child in building_dictionary.transform)
        {
            Ser_Building_Placement building_placement = new Ser_Building_Placement();

            building_placement.building_prefab_name = child.name.Replace("(Clone)", "");
            building_placement.building_global_position = child.position;
            building_placement.building_start_position = child.gameObject.GetComponent<PlaceableObject>().Start_Pos;
            building_placement.building_size = child.gameObject.GetComponent<PlaceableObject>().Size;

            buildings.buildings.Add(building_placement);
        }

        //string buildings_json = JsonUtility.ToJson(buildings);

        /// Road positions
        Ser_Road_Positions road_positions = new Ser_Road_Positions();
        List<Vector3Int> road_position_tiles = road_network.get_all_road_positions();

        foreach(Vector3Int position in road_position_tiles)
        {
            road_positions.road_tiles.Add(position);
        }

        //string road_positions_json = JsonUtility.ToJson(road_positions);    

        /// Mark tiles
        Ser_Tilemap_Markzone tilemap_markzone = new Ser_Tilemap_Markzone();

        List<(Vector3Int, int)> positions_and_indexes = markzone_tilemap_controller.get_tile_positions_and_indexes();

        foreach ((Vector3Int pos, int ind) in positions_and_indexes)
        {
            if (ind == 0)
            {
                tilemap_markzone.tiles_0.Add(pos);
            }
            else if (ind == 1)
            {
                tilemap_markzone.tiles_1.Add(pos);
            }
            else if (ind == 2)
            {
                tilemap_markzone.tiles_2.Add(pos);
            }
        }

        //string tilemap_markzone_json = JsonUtility.ToJson(tilemap_markzone);

        Ser_Game_State game_state = new Ser_Game_State();
        game_state.buildings = buildings;
        game_state.road_positions = road_positions;
        game_state.tilemap_markzone = tilemap_markzone;

        string game_state_json = JsonUtility.ToJson(game_state);

        /// Save to disk
        string save_path = Application.persistentDataPath + "\\" + load_save_name;
        if (!Directory.Exists(save_path))
        {
            DirectoryInfo dir = Directory.CreateDirectory(save_path);
        }

        System.IO.File.WriteAllText(save_path + "\\Game_State.json", game_state_json);
    }

    [SerializeField] private ConstructionDictionary construction_dictionary;
    [SerializeField] private TilemapController main_tilemap_controller;
    [SerializeField] private BuildingDictionary buildings_dictionary;
    [SerializeField] private GameObject grass_mask;
    [SerializeField] private TreeManager tree_manager;
    [SerializeField] private BuildingSystem building_system;

    private void load_state()
    {
        string game_state_json = System.IO.File.ReadAllText(Application.persistentDataPath + "\\" + load_save_name + "\\Game_State.json");

        Ser_Game_State game_state = JsonUtility.FromJson<Ser_Game_State>(game_state_json);

        /// Add buildings
        Dictionary<string, ConstructionInfo> construction_info_dictionary = new Dictionary<string, ConstructionInfo>();
        foreach (ConstructionInfo construction_info in construction_dictionary.get_construction_elements())
        {
            construction_info_dictionary[construction_info.construction_name] = construction_info;
        }

        foreach (var building_placement in game_state.buildings.buildings)
        {
            GameObject prefab = construction_info_dictionary[building_placement.building_prefab_name].gameObject;

            Vector3 position = building_placement.building_global_position;

            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            obj.transform.parent = building_dictionary.transform;
            PlaceableObject objectToPlace = obj.GetComponent<PlaceableObject>();
            objectToPlace.tilemap_controller = main_tilemap_controller;
            objectToPlace.Start_Pos = building_placement.building_start_position;
            objectToPlace.Size = building_placement.building_size;

            objectToPlace.Place();
            main_tilemap_controller.take_area(objectToPlace, 0);

            buildings_dictionary.add_building(obj.GetComponent<Building>());
        }

        /// Add roads
        road_network.add_roads(game_state.road_positions.road_tiles, RoadNetwork.Road_Placement_Mode.Permanent);
        main_tilemap_controller.take_area(game_state.road_positions.road_tiles, 0);

        /// Add markzones
        markzone_tilemap_controller.take_area(game_state.tilemap_markzone.tiles_0, 0);
        main_tilemap_controller.take_area(game_state.tilemap_markzone.tiles_0, 0);

        markzone_tilemap_controller.take_area(game_state.tilemap_markzone.tiles_1, 1);
        main_tilemap_controller.take_area(game_state.tilemap_markzone.tiles_0, 0);

        markzone_tilemap_controller.take_area(game_state.tilemap_markzone.tiles_2, 2);
        main_tilemap_controller.take_area(game_state.tilemap_markzone.tiles_0, 0);

        markzone_tilemap_controller.instantiate_prefab_in_all_non_empty_tiles(grass_mask);

        /// Final touches
        tree_manager.recalculate_visible_trees(main_tilemap_controller);
        building_system.rebuild_nav_meshes();
        
        StartCoroutine(rebuild_nav_meshes_late());
    }

    private IEnumerator rebuild_nav_meshes_late()
    {
        //yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(1.0f);

        building_system.rebuild_nav_meshes();
    }

    public static string load_save_name = "default save";
    public static bool loaded = true;
    //private bool scene_activated = false;

    void Start()
    {
        //scene_activated = true;
    }

    void Update()
    {
        if (!loaded/* && !scene_activated*/)
        {
            load_state();
            loaded = true;
        }

    }

}

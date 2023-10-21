/// 3D Grid Building System - Unity Tutorial | City Builder, RTS, Factorio
/// By: Tamara Makes Games
/// https://www.youtube.com/watch?v=rKp9fWvmIww

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.InputSystem;
using Unity.AI.Navigation;

/**

    Building System
    This component let us put a variety of interactions with the tiles.
    In the next section we're discussing the steps it takes to complete the construction and placement of various types
    of blocks. The reason is if we change or cancel the current construction placement (e.g. changing from placing
    buildings to painting tiles), we need to CANCEL THE CURRENT OPERATION AT ANY TIME (its like we are in the middle of
    a "transaction" and we need to revert all non-commited changes) and revert to a safe state.


    Construction modes:

        1.- Block placement (e.g. buildings)
            a) Select block to place 
            b) - Move the block where the mouse is pointed
               - Rotate it (optional)
            c) Place the block if the space is available

        2.- Zone painting (to place proc buildings for example)
            a) Hold down click on the initial painting spot
            b) Drag mouse while click is down to paint an area
            c) Stop pressing on the final painting spot

        3.- Road placement
            a) Click on the initial road spot
            b) Mirror the road placement (optional)
            c) Click on the end road spot


*/

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    public Vector3 grid_scale;

    [SerializeField] public Tilemap MainTilemap;
    [SerializeField] private TilemapController MainTilemap_controller;
    //[SerializeField] private TileBase whiteTile;

    [SerializeField] private Tilemap selection_tilemap;
    [SerializeField] private TilemapController selection_tilemap_controller;
    //[SerializeField] private TileBase selection_tile;

    [SerializeField] private Tilemap mark_tilemap;
    [SerializeField] private TilemapController mark_tilemap_controller;

    //[SerializeField] private GrassTilesManager grass_tiles_manager;
    [SerializeField] private TreeManager tree_manager;

    [SerializeField] private Material invalid_place_material;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceableObject objectToPlace;

    [SerializeField] private LayerMask ground_mask;

    public GameObject ground_tile;

    public enum Construction_Modes
    {
        None,
        Block_Placement,
        Zone_Painting,
        Road_Placement
    }

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
        grid_scale = grid.transform.localScale;
    }

    #endregion

    #region Construction mode selection

    [SerializeField] private PlayerInputHandler input_handler;

    private void exit_current_construction_mode()
    {
        input_handler.go_to_selection_mode();
    }

    #endregion

    #region Update NavMesh Surfaces

    /// TODO: think if its best to have a list of navmeshes to update or each navmesh should have its own name
    [SerializeField] private NavMeshSurface vehicle_roads_navmesh;
    [SerializeField] private NavMeshSurface walkable_ground_navmesh;

    public void rebuild_nav_meshes()
    {
        vehicle_roads_navmesh.BuildNavMesh();
        walkable_ground_navmesh.BuildNavMesh();
    }

    #endregion

    #region Place block or zone

    [SerializeField] private PlayerInputHandler player_input_handler;

    public void begin_placing_block_or_zone(GameObject to_place)
    {
        BuildingPlacement building_placement = to_place.GetComponent<BuildingPlacement>();

        if (building_placement == null)
        {
            Debug.LogError("Gameobject doesn't have a BuildingPlacement");
        }

        print(building_placement.get_building_placement_mode());

        switch (building_placement.get_building_placement_mode())
        {
        case Building_Placement_Mode.Standalone:
            player_input_handler.switch_to_block_placement_mode();
            InitializeWithObject(to_place);

            break;

        case Building_Placement_Mode.Zone:
            player_input_handler.switch_to_zone_painting_mode();
            tile_index = building_placement.get_tile_index();

            break;
        }
    }

    #endregion

    #region Block placement

    private void restore_block_placement_mode()
    {
        Destroy(objectToPlace.gameObject);
        objectToPlace = null;
    }

    public void select_object_A()
    {
        /// 'objectToPlace' must be null to select a prefab
        if (objectToPlace != null)
        {
            return;
        }

        InitializeWithObject(prefab1);
    }

    public void select_object_B()
    {
        /// 'objectToPlace' must be null to select a prefab
        if (objectToPlace != null)
        {
            return;
        }

        InitializeWithObject(prefab2);
    }

    /// Rotate the object to be placed
    public void rotate_object()
    {
        /// 'objectToPlace' must be not null
        if (objectToPlace == null)
        {
            return;
        }

        objectToPlace.Rotate();
    }

    /// Place the object on the map
    public void place_object()
    {
        /// 'objectToPlace' must be not null
        if (objectToPlace == null)
        {
            return;
        }

        /// Check if we can place the object (not occupying used space)
        /// TODO: mark in a custom material the object to show if it can be put down
        //if (CanBePlaced(objectToPlace))
        if (MainTilemap_controller.can_be_placed(objectToPlace))
        {
            objectToPlace.Place();
            //TakeArea(start, objectToPlace.Size);
            MainTilemap_controller.take_area(objectToPlace, 0);

            //grass_tiles_manager.recalculate_grass_mask();
            tree_manager.recalculate_visible_trees(MainTilemap_controller);

            rebuild_nav_meshes();

            add_building_to_dictionary(objectToPlace.gameObject);

            objectToPlace = null;

            exit_current_construction_mode();
        }
        /// If the space is taken, we destroy the object
        /// TODO: don't destory the object
        else
        {
            restore_block_placement_mode();

            exit_current_construction_mode();
        }
    }

    /// If we have a building to be placed and want to cancel the operation
    public void cancel_placement_or_exit_mode()
    {
        /// If there is no 'objectToPlace', then we exit the block construction mode
        if (objectToPlace == null)
        {
            exit_current_construction_mode();
            return;
        }

        /// ...and if there is, we only destroy the object
        Destroy(objectToPlace.gameObject);
        objectToPlace = null;
    }

    /// Make an instance of a prefab and adds some components to drag and place the prefab in the map
    private void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        //GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        GameObject obj = Instantiate(prefab, position + prefab.transform.position, Quaternion.identity);
        obj.transform.parent = building_dictionary.building_dictionary_gameobject.transform;
        objectToPlace = obj.GetComponent<PlaceableObject>();
        objectToPlace.tilemap_controller = MainTilemap_controller;
        TemporalMaterial temporal_material = obj.AddComponent<TemporalMaterial>();
        temporal_material.set_temporal_material(invalid_place_material);
        temporal_material.enabled = false;
        ObjectDrag drag = obj.AddComponent<ObjectDrag>();
        drag.placeable_object = objectToPlace;
        //drag.originalOffset = prefab.transform.position;
    }

    [SerializeField] BuildingDictionary building_dictionary;

    private void add_building_to_dictionary(GameObject building_gameobject)
    {
        Building building = building_gameobject.GetComponent<Building>();

        building_dictionary.add_building(building);
    }

    #endregion

    #region Zone painting

    private Vector3Int initial_painting_spot = Vector3Int.back;
    private Vector3Int final_painting_spot = Vector3Int.back;

    private void restore_paint_zone_mode()
    {
        initial_painting_spot = Vector3Int.back;
        //final_painting_spot = Vector3Int.back;

        if (painting_zone != null)
        {
            StopCoroutine(painting_zone);
            painting_zone = null;
        }
        
        selection_tilemap.GetComponent<TilemapController>().clear_all_tiles();
    }

    private int tile_index = 0;
    /*private void set_tile_index(int index)
    {
        tile_index = index;
    }*/

    public void start_paint_zone()
    {
        Vector3 mouse_world_position = GetMouseWorldPosition();

        /// If the ray didn't hit the plane surface, dont procede
        if (mouse_world_position == Vector3.zero)
        {
            restore_paint_zone_mode();
            return;
        }

        initial_painting_spot = gridLayout.WorldToCell(mouse_world_position);

        painting_zone = StartCoroutine(drag_paint_zone());
    }

    private Coroutine painting_zone;
    private IEnumerator drag_paint_zone()
    {
        while (true)
        {
            if (initial_painting_spot == Vector3Int.back)
            {
                restore_paint_zone_mode();

                yield break;
            }

            select_area_in_tilemap();

            yield return new WaitForEndOfFrame();
        }
    }

    [SerializeField] private GameObject grass_mask;

    public void end_paint_zone()
    {
        if (initial_painting_spot == Vector3Int.back)
        {
            restore_paint_zone_mode();
            return;
        }

        Vector3 mouse_world_position = GetMouseWorldPosition();

        if (mouse_world_position == Vector3.zero)
        {
            restore_paint_zone_mode();
            return;
        }

        StopCoroutine(painting_zone);
        painting_zone = null;

        final_painting_spot = gridLayout.WorldToCell(mouse_world_position);

        if (!MainTilemap_controller.can_be_placed(initial_painting_spot, final_painting_spot))
        {
            restore_paint_zone_mode();
            return;
        }

        MainTilemap_controller.take_area(initial_painting_spot, final_painting_spot, 0);
        mark_tilemap_controller.take_area(initial_painting_spot, final_painting_spot, tile_index);

        mark_tilemap_controller.instantiate_prefab_in_each_tile(initial_painting_spot, final_painting_spot, grass_mask);

        //grass_tiles_manager.recalculate_grass_mask();
        tree_manager.recalculate_visible_trees(MainTilemap_controller);

        rebuild_nav_meshes();

        restore_paint_zone_mode();
    }

    public void cancel_zone_painting_or_exit_mode()
    {
        /// Cancel painting if we were in the middle of painting a zone
        if (initial_painting_spot != Vector3Int.back)
        {
            restore_paint_zone_mode();
            return;
        }

        /// Exit mode otherwise
        input_handler.go_to_selection_mode();
    }

    private void select_area_in_tilemap()
    {
        /// To mark the selection area, we must first clear the previous selection
        //selection_tilemap.GetComponent<TilemapController>().clear_all_tiles();
        selection_tilemap_controller.clear_all_tiles();

        /// The current mouse position is where the selection area must end
        Vector3Int current_painting_spot = gridLayout.WorldToCell(GetMouseWorldPosition());

        /*/// Since we need the start position and the sizes to fill the selection area, we call a function that converts
        /// two opposite corners
        (Vector3Int start, Vector3Int size) = get_start_and_size_of_corners(initial_painting_spot, current_painting_spot);

        /// We fill the selection area in the selection tilemap (not the main tilemap)
        take_selection_area(start, size);*/

        selection_tilemap_controller.take_area(initial_painting_spot, current_painting_spot, tile_index);
    }

    #endregion

    #region Road placement

    public RoadNetwork road_network;

    private enum Road_Direction
    {
        Left,
        Right
    }
    private Road_Direction current_road_direction = Road_Direction.Left;

    private Vector3Int initial_road_spot = Vector3Int.back;

    private void restore_road_placement_mode()
    {
        initial_road_spot = Vector3Int.back;

        if (placing_road != null)
        {
            StopCoroutine(placing_road);
            placing_road = null;
        }
    }

    public void change_to_road_placement()
    {
        player_input_handler.switch_to_road_placement_mode();
    }
    
    public void start_road_placement()
    {
        Vector3 mouse_world_position = GetMouseWorldPosition();

        /// If the ray didn't hit the plane surface, dont procede
        if (mouse_world_position == Vector3.zero)
        {
            restore_road_placement_mode();
            return;
        }

        initial_road_spot = gridLayout.WorldToCell(mouse_world_position);

        placing_road = StartCoroutine(drag_road_placement());
    }

    public void rotate_road()
    {
        if (initial_road_spot == Vector3Int.back)
        {
            return;
        }

        if (current_road_direction == Road_Direction.Left)
        {
            current_road_direction = Road_Direction.Right;
        }
        else
        {
            current_road_direction = Road_Direction.Left;
        }
    }

    private Coroutine placing_road;
    private IEnumerator drag_road_placement()
    {
        while (true)
        {
            if (initial_road_spot == Vector3Int.back)
            {
                restore_road_placement_mode();
                yield break;
            }

            place_road(current_road_direction, RoadNetwork.Road_Placement_Mode.Temporal);

            yield return new WaitForEndOfFrame();
        }
    }

    public void end_road_placement()
    {
        if (initial_road_spot == Vector3Int.back)
        {
            restore_road_placement_mode();
            return;
        }

        StopCoroutine(placing_road);
        placing_road = null;

        Vector3Int current_road_spot = gridLayout.WorldToCell(GetMouseWorldPosition());

        Road_Section road_section = new Road_Section(initial_road_spot, current_road_spot, current_road_direction);
        List<Vector3Int> road_positions = road_section.get_road_tiles();

        if (!MainTilemap_controller.can_be_placed(road_positions))
        {
            restore_road_placement_mode();
            return;
        }

        road_network.add_roads(road_positions, RoadNetwork.Road_Placement_Mode.Permanent);

        MainTilemap_controller.take_area(road_positions, 0);

        //grass_tiles_manager.recalculate_grass_mask();
        tree_manager.recalculate_visible_trees(MainTilemap_controller);

        rebuild_nav_meshes();

        restore_road_placement_mode();
    }

    public void cancel_road_placement_or_exit_mode()
    {
        /// Cancel road placement if we were constructing a road
        if (initial_road_spot != Vector3Int.back)
        {
            restore_road_placement_mode();
            return;
        }

        /// Exit mode otherwise
        input_handler.go_to_selection_mode();
    }

    private class Road_Section
    {
        private Vector3Int road_begin = Vector3Int.back;
        private Vector3Int road_end = Vector3Int.back;
        private Road_Direction road_direction = Road_Direction.Left;

        public Road_Section(Vector3Int _road_begin, Vector3Int _road_end, Road_Direction _road_direction = Road_Direction.Left)
        {
            road_begin = _road_begin;
            road_end = _road_end;
            road_direction = _road_direction;
        }

        public List<Vector3Int> get_road_tiles()
        {
            if (road_begin == Vector3Int.back || road_end == Vector3Int.back)
            {
                return new List<Vector3Int>();
            }

            Vector3Int first_section_begin = Vector3Int.back;
            Vector3Int first_section_end = Vector3Int.back;
            Vector3Int second_section_begin = Vector3Int.back;
            Vector3Int second_section_end = Vector3Int.back;

            List<Vector3Int> road_tiles = new List<Vector3Int>();

            /// If the road is straight
            if (road_begin.x == road_end.x)
            {
                int min = Math.Min(road_begin.y, road_end.y);
                int max = Math.Max(road_begin.y, road_end.y);

                for (int i = min; i <= max; ++i)
                {
                    road_tiles.Add(new Vector3Int(road_begin.x, i, 0));
                }

                return road_tiles;
            }
            else if (road_begin.y == road_end.y)
            {
                int min = Math.Min(road_begin.x, road_end.x);
                int max = Math.Max(road_begin.x, road_end.x);

                for (int i = min; i <= max; ++i)
                {
                    road_tiles.Add(new Vector3Int(i, road_begin.y, 0));
                }
                    
                return road_tiles;
            }

            /// In other case, the road has a turn
            Vector3Int corner_tile;
            if (road_direction == Road_Direction.Left)
            {
                corner_tile = new Vector3Int(road_begin.x, road_end.y, 0);
            }
            else
            {
                corner_tile = new Vector3Int(road_end.x, road_begin.y, 0);
            }
            
            /// Since there are two stright lines between the corner and the road begin/end, we call this function
            /// again two times (and delete the corner tile from one of them so it's not repeated)
            Road_Section section_begin_to_corner = new Road_Section(road_begin, corner_tile);
            List<Vector3Int> road_tiles_begin_to_corner = section_begin_to_corner.get_road_tiles();

            Road_Section section_end_to_corner = new Road_Section(corner_tile, road_end);
            List<Vector3Int> road_tiles_end_to_corner = section_end_to_corner.get_road_tiles();
            road_tiles_end_to_corner.Remove(corner_tile);
            
            road_tiles.AddRange(road_tiles_begin_to_corner);
            road_tiles.AddRange(road_tiles_end_to_corner);
           
           return road_tiles;

        }
    }

    private List<Vector3Int> place_road(Road_Direction direction, RoadNetwork.Road_Placement_Mode placement_mode)
    {
        /// The current mouse position is where the selection area must end
        Vector3Int current_road_spot = gridLayout.WorldToCell(GetMouseWorldPosition());

        Road_Section road_section = new Road_Section(initial_road_spot, current_road_spot, direction);
        List<Vector3Int> road_tiles = road_section.get_road_tiles();

        road_network.add_roads(road_tiles, placement_mode);

        return road_tiles;
    }


    #endregion

    #region Utils

    // Lanzamos un rayo para saber en que coordenadas del mundo tocó nuestro mouse (tiene que colisionar con un objeto)
    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        /// TODO: put a layer mask to filter buildings
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, ground_mask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    /// Devuelve las coordenadas del centro de la celda mas cercana de una posición determinada
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        Vector3 newPos = grid.GetCellCenterWorld(cellPos);

        return newPos;
    }

    /// Devuelve el tipo de celdas de un área determinada
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            ++counter;
        }

        return array;

    }

    #endregion

}

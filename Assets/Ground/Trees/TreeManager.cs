using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    
    //[SerializeField] private List<GameObject> tree_spawners;
    [SerializeField] private List<TreeSpawner> tree_spawners;
    [SerializeField] private List<GameObject> tree_prefabs;

    private Dictionary<Vector3Int, List<GameObject>> tree_dictionary = new Dictionary<Vector3Int, List<GameObject>>();

    private static System.Random random = new System.Random(0);

    [SerializeField] private BuildingSystem building_system;

    void Start()
    {
        List<GameObject> tree_spawners_gameobjects = new List<GameObject>();
        foreach (TreeSpawner tree_spawner in tree_spawners)
        {
            tree_spawners_gameobjects.AddRange(tree_spawner.get_tree_spawners());
        }

        foreach (GameObject spawner in tree_spawners_gameobjects)
        {
            GameObject tree_prefab = tree_prefabs[random.Next(tree_prefabs.Count)];

            GameObject instance = Instantiate(tree_prefab, spawner.transform.position, Quaternion.identity);
            instance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            instance.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, (float)random.NextDouble() * 360.0f, 0.0f));
            instance.transform.parent = gameObject.transform;

            Vector3Int tree_tile_position  = building_system.gridLayout.WorldToCell(spawner.transform.position);
            List<GameObject> trees_in_tile;

            if (tree_dictionary.TryGetValue(tree_tile_position, out trees_in_tile))
            {
                trees_in_tile.Add(instance);
            }
            else
            {
                trees_in_tile = tree_dictionary[tree_tile_position] = new List<GameObject>();
                trees_in_tile.Add(instance);
            }
        }
    }

    //[SerializeField] private TilemapController main_tilemap_controller;

    public void recalculate_visible_trees(TilemapController tilemap_controller)
    {
        List<Vector3Int> occupied_positions_tiles = tilemap_controller.get_non_empty_tiles_positions();

        foreach (Vector3Int occupied_tile in occupied_positions_tiles)
        {
            List<GameObject> trees_in_tile;
            if (tree_dictionary.TryGetValue(occupied_tile, out trees_in_tile))
            {
                foreach (GameObject tree in trees_in_tile)
                {
                    tree.SetActive(false);
                }
            }
        }
    }

}

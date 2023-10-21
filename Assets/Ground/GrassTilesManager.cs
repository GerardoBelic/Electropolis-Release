using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassTilesManager : MonoBehaviour
{
    
    [SerializeField] private Tilemap main_tilemap;
    [SerializeField] private TilemapController main_tilemap_controller;

    //private Texture2D grass_mask;
    private Vector3Int tilemap_size;

    [SerializeField] private Material grass_material;

    void Start()
    {
        tilemap_size = main_tilemap_controller.get_size_tilemap();
        //grass_mask = new Texture2D(tilemap_size.x, tilemap_size.y);
    }

    public void recalculate_grass_mask()
    {
        Texture2D new_grass_mask = new Texture2D(tilemap_size.x, tilemap_size.y);
        var fill_color = Color.black;
        var fill_color_array = new Color[tilemap_size.x * tilemap_size.y];

        for (int i = 0; i < fill_color_array.Length; ++i)
        {
            fill_color_array[i] = fill_color;
        }

        new_grass_mask.SetPixels(fill_color_array);
        new_grass_mask.Apply();
        

        List<Vector3Int> non_occupied_positions_tiles = main_tilemap_controller.get_empty_zeroed_tiles_positions();

        foreach (Vector3Int tile in non_occupied_positions_tiles)
        {
            new_grass_mask.SetPixel(tile.x, tile.y, Color.white);
        }

        new_grass_mask.Apply();

        grass_material.SetTexture("_NoGrassTex", new_grass_mask);
    }

}

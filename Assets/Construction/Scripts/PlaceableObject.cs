/// 3D Grid Building System - Unity Tutorial | City Builder, RTS, Factorio
/// By: Tamara Makes Games
/// https://www.youtube.com/watch?v=rKp9fWvmIww

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaceableObject : MonoBehaviour
{
    
    public bool Placed { get; private set; }
    public Vector3Int Size { get; set; }
    private Vector3[] Vertices;
    private Bounds bounds;
    public Vector3Int Start_Pos { get; set; }

    private List<GameObject> sidewalk_base = new List<GameObject>();
    public static float sidewalk_offset = 0.5f; /// Magic number that represents the height of the sidewalk

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3( b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3( b.size.x, -b.size.y,  b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y,  b.size.z) * 0.5f;

        foreach(var v in Vertices)
        {
            //print(v);
        }
    }

    private void CalculateSizeInCells()
    {
        //Vector3Int[] vertices = new Vector3Int[Vertices.Length];
        Vector3[] worldspace_vertices = new Vector3[Vertices.Length];

        for (int i = 0; i < worldspace_vertices.Length; ++i)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            worldspace_vertices[i] = worldPos;
            //vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        //Size = new Vector3Int(Math.Abs((vertices[0] - vertices[1]).x), Math.Abs((vertices[0] - vertices[3]).y), 1);
        Vector3 true_size = new Vector3(Math.Abs((worldspace_vertices[0] - worldspace_vertices[1]).x), 0.0f, Math.Abs((worldspace_vertices[0] - worldspace_vertices[3]).z));
        Size = BuildingSystem.current.gridLayout.WorldToCell(true_size) + new Vector3Int(0, 0, 1);
    }

    private void GetVerticesBoundingBox()
    {
        var renderers = gameObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            //return new Bounds(g.transform.position, Vector3.zero);
            Debug.LogError("Could not calculate gameobject bounds");
        }

        var b = renderers[0].bounds;

        foreach (Renderer r in renderers)
        {
            b.Encapsulate(r.bounds);
        }

        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3( b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3( b.size.x, -b.size.y,  b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y,  b.size.z) * 0.5f;

        foreach(var v in Vertices)
        {
            //print(v);
        }

        bounds = b;
    }

    private void CalculateBoundingBoxSizeInCells()
    {
        Vector3 true_size = new Vector3(Math.Abs((Vertices[0] - Vertices[1]).x), 0.0f, Math.Abs((Vertices[0] - Vertices[3]).z));
        Size = BuildingSystem.current.gridLayout.WorldToCell(true_size) + new Vector3Int(0, 0, 1);
        //print(Size);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    private void Awake()
    {
        //GetColliderVertexPositionsLocal();
        //CalculateSizeInCells();
        /// Alternative to the above instructions (they depend on a BoxCollider,
        /// while the below don't depend on anything)
        GetVerticesBoundingBox();
        CalculateBoundingBoxSizeInCells();
    }

    private void Start()
    {
        temporal_material = gameObject.GetComponent<TemporalMaterial>();
    }

    [HideInInspector] public TilemapController tilemap_controller;
    private TemporalMaterial temporal_material;

    void Update()
    {
        if (temporal_material == null)
        {
            return;
        }
        
        if (tilemap_controller.can_be_placed(this))
        {
            temporal_material.enabled = false;
        }
        else if (!tilemap_controller.can_be_placed(this))
        {
            temporal_material.enabled = true;
        }
    }

    public void Rotate()
    {

        transform.Rotate(new Vector3(0, 90, 0));
        Size = new Vector3Int(Size.y, Size.x, 1);

        Vector3[] vertices = new Vector3[Vertices.Length];
        for (int i = 0; i < vertices.Length; ++i)
        {
            vertices[i] = Vertices[(i + 1) % Vertices.Length];
        }

        Vertices = vertices;

    }

    private void add_foundations()
    {
        /// Construct the ground under the construction
        for (int i = 0; i <= Size.x; ++i)
        {
            for (int j = 0; j <= Size.y; ++j)
            {
                Vector3Int offset = new Vector3Int(i, j, 0);
                Vector3 gameobject_position = BuildingSystem.current.MainTilemap.GetCellCenterWorld(Start_Pos + offset);
                gameobject_position.y = 0.0f;

                GameObject ground_instance = Instantiate(BuildingSystem.current.ground_tile, gameobject_position, Quaternion.identity);
                ground_instance.transform.parent = this.transform;

                sidewalk_base.Add(ground_instance);
            }
        }
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        if (drag != null)
            Destroy(drag);

        Placed = true;

        add_foundations();

        if (temporal_material != null)
            Destroy(temporal_material);

        temporal_material = null;
    }

}

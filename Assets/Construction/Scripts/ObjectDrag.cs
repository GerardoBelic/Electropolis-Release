/// 3D Grid Building System - Unity Tutorial | City Builder, RTS, Factorio
/// By: Tamara Makes Games
/// https://www.youtube.com/watch?v=rKp9fWvmIww

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 originalOffset;    // la posición de los objetos puede ser ajustada en su prefab, por lo que es necesario conservar la posición original
    private Vector3 offset;
    //public Vector3Int size;
    public PlaceableObject placeable_object;

    /*// Cuando apretamos el mouse, guardamos la distancia que hay entre el centro del objeto y el puntero del ratón en el mundo
    private void OnMouseDown()
    {
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();
    }

    // Cuando arrastremos el mouse, moveremos la posición del objeto al centro de la celda mas cercana al puntero del mouse (tomando en cuenta el offset de OnMouseDown())
    private void OnMouseDrag()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition() + offset;
        
        Vector3 snapedPos = BuildingSystem.current.SnapCoordinateToGrid(pos);
        snapedPos.y = originalOffset.y;

        transform.position = snapedPos;
    }*/

    // Debido a que el centro del objeto no es igual a la posición del prefab, es preferente guardar esta última para que los objetos no se metan debajo del suelo
    void Awake()
    {
        originalOffset = transform.position;
    }

    void Start()
    {
        /// Check that we assigned the placeable object
        if (placeable_object == null)
        {
            Debug.LogError("Placeable object not assigned");
        }
    }

    private Vector3Int calculate_start_position(Vector3 snaped_pos)
    {
        Vector3Int size = placeable_object.Size + new Vector3Int(1, 1, 0);
        Vector3Int half_size = size / 2;

        Vector3Int pos_snaped_cell = BuildingSystem.current.gridLayout.WorldToCell(snaped_pos);
        pos_snaped_cell -= half_size;

        return pos_snaped_cell;
    }

    void Update()
    {
        Vector3 pos = BuildingSystem.current.GetMouseWorldPosition();

        /// If the size of the object is even in 'x' and/or 'y', then we must offset the position in 'x'/'y'
        Vector3 even_size_offset = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3Int size = placeable_object.Size;
        if ((size.x + 1) % 2 == 0)
        {
            even_size_offset.x += -BuildingSystem.current.grid_scale.x / 2.0f;
        }
        if ((size.y + 1) % 2 == 0)
        {
            even_size_offset.z += -BuildingSystem.current.grid_scale.y / 2.0f;
        }

        /// First snap the mouse world position to the grid
        Vector3 snapedPos = BuildingSystem.current.SnapCoordinateToGrid(pos);
        /// Next adjust the height of the prefab in case it is not adjusted to zero
        snapedPos.y = originalOffset.y;
        /// Then, apply an offset in case the prefab has any of its sides even
        snapedPos += even_size_offset;
        /// Finally apply the height offset caused by the ground foundation
        snapedPos.y += PlaceableObject.sidewalk_offset;

        transform.position = snapedPos;

        /// Calculate the start position of the placeable
        placeable_object.Start_Pos = calculate_start_position(snapedPos);
    }
}

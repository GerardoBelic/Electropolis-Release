using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOccupiedSpace
{

    List<Vector3Int> get_occupied_space();

    void set_occupied_space(List<Vector3Int> tiles);

}

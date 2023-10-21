using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISurroundRoads
{

    List<Vector3Int> get_surround_roads();

    void set_surround_roads(List<Vector3Int> roads);

}

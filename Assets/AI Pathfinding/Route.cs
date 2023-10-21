using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Route_Type
{
    Walk,
    Walk_And_Drive
}

public enum Route_Phase
{
    Walk,
    Drive,
    Teleport_To_Walk,
    Teleport_To_Drive,
    Finished,
    Going
}

public class Route
{
    /// Start route
    private Transform route_origin = null; /// The starting point of the route

    /// For walk/drive route type
    /*private Transform sidewalk_teleporter_to_road = null;    /// First destination
    private Transform road_teleporter_origin = null; /// Car spawn point
    private Transform road_teleporter_destination = null; /// To reach the sidewalk of the destination target
    private Transform sidewalk_teleporter_from_road = null; /// Human spawn point closest to destination target*/

    /// For walk and walk/drive route types
    private Transform building_entrance_destination = null;  /// The destination target

    /// For the compas
    //private Transform route_target = null;

    /// The building target
    private Building building_target;

    /// Route type
    private Route_Type route_type;

    /// Walk route constructor
    /*public Route(Transform origin, Transform building_entrance, Building target)
    {
        route_origin = origin;
        building_entrance_destination = building_entrance;

        building_target = target;

        route_type = Route_Type.Walk;
    }*/

    /// Walk/drive route constructor
    /*public Route(Transform origin, Transform first_sidewalk_teleporter, Transform first_road_teleporter,
                 Transform second_road_teleporter, Transform second_sidewalk_teleporter, Transform building_entrance,
                 Building target)
    {
        route_origin = origin;

        sidewalk_teleporter_to_road = first_sidewalk_teleporter;
        road_teleporter_origin = first_road_teleporter;
        road_teleporter_destination = second_road_teleporter;
        sidewalk_teleporter_from_road = second_sidewalk_teleporter;
        building_entrance_destination = building_entrance;

        building_target = target;

        route_type = Route_Type.Walk_And_Drive;
    }*/

    public Route(Transform origin, Transform target)
    {
        route_origin = origin;

        building_entrance_destination = target;
    }

    public Transform get_starting_point()
    {
        return route_origin;
    }
    
    public Building get_target_building()
    {
        return building_target;
    }

    public (Route_Phase, Transform) consume_single_route_phase()
    {
        if (building_entrance_destination != null)
        {
            Transform building_entrance = building_entrance_destination;

            building_entrance_destination = null;

            return (Route_Phase.Going, building_entrance);
        }

        /// If there there is no more building_entrance_destination, we reached the building (or the reference vanished)
        return (Route_Phase.Finished, null);
    }

    /*private (Route_Phase, Transform) consume_walk_type_route_phase()
    {
        if (building_entrance_destination != null)
        {
            Transform building_entrance = building_entrance_destination;

            building_entrance_destination = null;

            return (Route_Phase.Walk, building_entrance);
        }

        /// If there there is no more building_entrance_destination, we reached the building (or the reference vanished)
        return (Route_Phase.Finished, null);
    }

    private (Route_Phase, Transform) consume_walk_and_drive_type_route_phase()
    {
        if (sidewalk_teleporter_to_road != null)
        {
            Transform first_sidewalk_teleporter = sidewalk_teleporter_to_road;

            sidewalk_teleporter_to_road = null;

            return (Route_Phase.Walk, first_sidewalk_teleporter);
        }
        else if (road_teleporter_origin != null)
        {
            Transform first_road_teleporter = road_teleporter_origin;

            road_teleporter_origin = null;

            return (Route_Phase.Teleport_To_Drive, first_road_teleporter);
        }
        else if (road_teleporter_destination != null)
        {
            Transform second_road_teleporter = road_teleporter_destination;

            road_teleporter_destination = null;

            return (Route_Phase.Drive, second_road_teleporter);
        }
        else if (sidewalk_teleporter_from_road != null)
        {
            Transform second_sidewalk_teleporter = sidewalk_teleporter_from_road;

            sidewalk_teleporter_from_road = null;

            return (Route_Phase.Teleport_To_Walk, second_sidewalk_teleporter);
        }
        else if (building_entrance_destination != null)
        {
            Transform building_entrance = building_entrance_destination;

            building_entrance_destination = null;

            return (Route_Phase.Walk, building_entrance);
        }

        /// If there there is no more building_entrance_destination, we reached the building (or the reference vanished)
        return (Route_Phase.Finished, null);
    }

    public (Route_Phase, Transform) consume_route_phase()
    {
        if (route_type == Route_Type.Walk)
        {
            return consume_walk_type_route_phase();
        }

        /// Else if route_type == Route_Type.Walk_And_Drive
        return consume_walk_and_drive_type_route_phase();
    }*/

}

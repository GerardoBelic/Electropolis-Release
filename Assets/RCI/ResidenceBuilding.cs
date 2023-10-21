using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidenceBuilding : Building, IHomeOccupation
{

    #region Home occupation

    [SerializeField] private int home_total_slots = 0;
    private int home_reserved_slots = 0;
    private HashSet<Citizen> residents_in_building = new HashSet<Citizen>();

    public bool are_home_slots_available()
    {
        if (home_total_slots - home_reserved_slots > 0)
        {
            return true;
        }

        return false;
    }

    public void request_home_vacancy()
    {
        ++home_reserved_slots;
    }

    public void release_home_vacancy()
    {
        --home_reserved_slots;
    }

    public void start_lodging(Citizen citizen)
    {
        residents_in_building.Add(citizen);
    }

    public void stop_lodging(Citizen citizen)
    {
        residents_in_building.Remove(citizen);
    }

    public int get_total_home_slots()
    {
        return home_total_slots;
    }

    #endregion

}

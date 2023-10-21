using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHomeOccupation
{
    
    /// To know if a citizen can reside in a house
    bool are_home_slots_available();

    /// Request to take a slot
    void request_home_vacancy();

    /// Release vacancy slot
    void release_home_vacancy();

    /// Notify that a citizen has entered the home building
    void start_lodging(Citizen citizen);

    /// Notify that a citizen has left the home building
    void stop_lodging(Citizen citizen);


    /// Get max number of home slots
    int get_total_home_slots();

}
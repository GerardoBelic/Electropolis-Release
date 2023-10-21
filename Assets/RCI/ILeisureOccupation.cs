using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILeisureOccupation
{
    
    /// To know if a citizen can enter the building
    bool are_leisure_places_available();

    /// Request to take a slot
    void request_leisure_vacancy();

    /// Release vacancy slot
    void release_leisure_vacancy();

    /// Notify that a citizen has entered the building
    void start_amusement(Citizen citizen);

    /// Notify that a citizen has left the building
    void stop_amusement(Citizen citizen);

}

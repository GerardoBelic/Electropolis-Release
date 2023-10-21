using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Power_Consumption
{
    
    /// Updates the consumption rate of electricity
    void update_electricity_consumption();

    /// Calculates the electricity consumption based on the building occupation
    float get_electricity_consumption();
    
}

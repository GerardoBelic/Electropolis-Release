using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Water_Consumption
{

    /// Updates the consumption rate of water/sewage
    void update_water_consumption();

    /// Calculates the water consumption based on the building occupation
    float get_water_consumption();

}

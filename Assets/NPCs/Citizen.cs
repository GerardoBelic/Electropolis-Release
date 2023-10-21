using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**

    Citizens are NPCs with jobs and needs of entertainment

    Jobs:
    -Have a job (search the highest paying job/closest one)

    Entertainment:
    -Each citizen has a hapiness bar that indicates its performance at job
    -The hapiness is filled by going to commerces andrecreational sites

    Avatars:
    -A citizen needs one avatar (with an animation controller) to visualize movement between buildings
    

*/

public class Citizen : MonoBehaviour
{
    
    #region Get unique ID for citizen

    public int citizen_id { get; private set; }

    private static int citizen_id_counter = 0;

    private static int get_unique_citizen_id()
    {
        return citizen_id_counter++;
    }

    #endregion

    #region Operator overloads for dictionary lookup

    public override int GetHashCode()             
    {  
        return citizen_id; 
    }

    public override bool Equals(object other) 
    { 
        Citizen other_citizen = other as Citizen;

        return this.citizen_id == other_citizen.citizen_id; 
    }

    #endregion

}

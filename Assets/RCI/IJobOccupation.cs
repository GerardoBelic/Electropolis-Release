using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJobOccupation
{
    
    /// To know if a citizen can work in the building
    bool are_job_places_available();

    /// Request to take a slot (and generate a schedule)
    (Schedule, Schedule) request_job_vacancy();

    /// Release vacancy slot
    void release_job_vacancy();

    /// Notify that a citizen has entered the building
    void start_working(Citizen citizen);

    /// Notify that a citizen has left the building
    void stop_working(Citizen citizen);


    /// Get max number of job slots
    int get_total_job_slots();

    /// Get money generated per hour per job
    int get_money_geneated_per_hour_per_job();

}

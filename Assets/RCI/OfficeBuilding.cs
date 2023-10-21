using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeBuilding : Building, IJobOccupation
{
    
    private void initialize_schedule_generator()
    {
        /// Get the schedule generator
        job_schedule_generator = gameObject.GetComponent<ScheduleGenerator>();
        if (job_schedule_generator == null)
        {
            Debug.LogError("No schedule generator found");
        }
    }

    void Awake()
    {
        initialize_building_placement();
        initialize_building_entrance();
        //initialize_schedule_generator();

        id_building = get_unique_building_id();
    }

    #region Job occupation

    [SerializeField] private int job_total_slots = 0;
    private int job_reserved_slots = 0;
    private HashSet<Citizen> workers_in_building = new HashSet<Citizen>();

    private ScheduleGenerator job_schedule_generator;

    public bool are_job_places_available()
    {
        if (job_total_slots - job_reserved_slots > 0)
        {
            return true;
        }

        return false;
    }

    public (Schedule, Schedule) request_job_vacancy()
    {
        ++job_reserved_slots;

        (Schedule start_workday_schedule, Schedule end_workday_schedule) = job_schedule_generator.generate_schedule();

        return (start_workday_schedule, end_workday_schedule);
    }

    public void release_job_vacancy()
    {
        --job_reserved_slots;
    }

    public void start_working(Citizen citizen)
    {
        workers_in_building.Add(citizen);
    }

    public void stop_working(Citizen citizen)
    {
        workers_in_building.Remove(citizen);
    }

    public int get_total_job_slots()
    {
        return job_total_slots;
    }

    [SerializeField] private int money_geneated_per_hour_per_job = 0;
    public int get_money_geneated_per_hour_per_job()
    {
        return money_geneated_per_hour_per_job;
    }

    #endregion
    
}

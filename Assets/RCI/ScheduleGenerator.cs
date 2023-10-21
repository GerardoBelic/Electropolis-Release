using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ScheduleGenerator : MonoBehaviour
{

    [System.Serializable]
    public struct Hour
    {
        [Range(0, 23)]
        public byte hour;
        [Range(0, 59)]
        public byte minute;

        public bool cmp_equal(Hour other)
        {
            return (this.hour == other.hour && this.minute == other.minute);
        }
    }

    [System.Serializable]
    public struct Workday
    {
        public Hour start_hour;
        public Hour end_hour;
    }

    [SerializeField] private List<Workday> workdays;

    [Range(1, 7)]
    [SerializeField] private int schedule_days = 1;

    private List<Days> generate_random_days()
    {
        List<Days> days = Enum.GetValues(typeof(Days)).Cast<Days>().ToList();

        System.Random random = new System.Random();

        int days_remaining = 7 - schedule_days;

        while (days_remaining > 0)
        {
            --days_remaining;

            int random_index = random.Next(0, days.Count);

            days.RemoveAt(random_index);
        }

        return days;
    }

    public (Schedule, Schedule) generate_schedule()
    {
        List<Days> schedule_days = generate_random_days();

        System.Random random = new System.Random();
        int random_index = random.Next(0, workdays.Count);
        Workday workday = workdays[random_index];

        Alarm start_alarm = new Alarm(schedule_days, workday.start_hour.hour, workday.start_hour.minute);
        Schedule start_schedule = new Schedule();
        start_schedule.add_alarm(start_alarm);

        Alarm end_alarm = new Alarm(schedule_days, workday.end_hour.hour, workday.end_hour.minute);
        Schedule end_schedule = new Schedule();
        end_schedule.add_alarm(end_alarm);

        return (start_schedule, end_schedule);
    }

    void Awake()
    {
        /// Check that we assigned the start and end of the schedule
        foreach (Workday workday in workdays)
        {
            if (workday.start_hour.cmp_equal(workday.end_hour))
            {
                Debug.LogError("Start/end hour are the same");
            }
        }
    }

}

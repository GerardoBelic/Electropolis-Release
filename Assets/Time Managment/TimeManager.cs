using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

/// An alarm triggers at an specific hour and minute, for one or more days a week
public class Alarm
{
    private HashSet<Days> alarm_days;
    /// The day hoes from 00:00 (0.0) to 23:59 (1.0), and its easier to work with just one time measure
    private double alarm_time = -1.0;

    public double get_alarm_time()
    {
        return alarm_time;
    }

    public bool is_day_in_alarm(Days day)
    {
        return alarm_days.Contains(day);
    }

    public Alarm(List<Days> days, byte hour, byte minute)
    {
        alarm_days = new HashSet<Days>(days);
        
        alarm_time = (hour / 24) + (minute / (60 * 24));
    }

    /// To help Schedule compare two sets of alarms, we override Equals()
    public override bool Equals(object other)
    {
        Alarm other_alarm = other as Alarm;

        if (other_alarm == null)
        {
            return false;
        }

        /// Compare the alarms time
        if (this.alarm_time != other_alarm.alarm_time)
        {
            return false;
        }

        /// Compare if the number of days is the same in both lists
        if (this.alarm_days.Count != other_alarm.alarm_days.Count)
        {
            return false;
        }

        /// Compare the days in both hash sets
        if (!this.alarm_days.SetEquals(other_alarm.alarm_days))
        {
            return false;
        }

        /// In other case the two alarms are equal
        return true;
    }

    /// We also need to override GetHashCode()
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 19;

            foreach (Days day in alarm_days)
            {
                hash = hash * 31 + day.GetHashCode();
            }

            hash = hash * 29 + alarm_time.GetHashCode();

            return hash;
        }
    }
}

/// This class stores multiple alarms if a task is requiered to perform at multiple
/// hours in multiple days
public class Schedule
{
    private HashSet<Alarm> schedule_alarms = new HashSet<Alarm>();

    /*public Schedule(List<Alarm> alarms)
    {
        schedule_alarms = alarms;
    }*/

    public void add_alarm(Alarm alarm)
    {
        schedule_alarms.Add(alarm);
    }

    public List<Alarm> get_alarms()
    {
        return schedule_alarms.ToList();
    }

    /// Since this class is used as key in a dictionary, we must override Equals() and GetHashCode()
    public override bool Equals(object other)
    {
        Schedule other_schedule = other as Schedule;

        if (other_schedule == null)
        {
            return false;
        }

        /// Compare the size ot the two alarms hash sets
        if (this.schedule_alarms.Count != other_schedule.schedule_alarms.Count)
        {
            return false;
        }

        /// Compare the two hash sets
        if (!this.schedule_alarms.SetEquals(other_schedule.schedule_alarms))
        {
            return false;
        }

        /// In any other case, the two schedules are the same
        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 19;

            foreach (Alarm alarm in schedule_alarms)
            {
                hash = hash * 31 + alarm.GetHashCode();
            }

            return hash;
        }
    }
}

public class TimeManager : MonoBehaviour
{

    /// Current day
    private DateTime current_date = new DateTime(2122, 12, 31, 8, 24, 0);

    public DateTime get_current_date()
    {
        return current_date;
    }

    /// Stores the time passed in-game. Every unit is a day passed.
    private double current_time = 0.35;

    public double get_current_time()
    {
        //return current_time;

        double day_time = current_time - Math.Truncate(current_time);

        return day_time;
    }

    /// Ratio conversor between real time and in-game time (e.g. 24.0 means 1 hour IRL equals 24 hours ingame)
    private double real_to_ingame_time_ratio = 24.0 / 60.0 / 60.0;

    /// For every schedule there is an event to be invoked
    private Dictionary<Schedule, UnityEvent> schedule_events = new Dictionary<Schedule, UnityEvent>();

    /// Creates and event for a schedule and returns it to the caller to be subscribed
    public UnityEvent get_schedule_event(Schedule schedule)
    {
        UnityEvent dictionary_event;

        /// If the schedule already exists, return its associated event
        if (schedule_events.TryGetValue(schedule, out dictionary_event))
        {
            return dictionary_event;
        }

        /// If there is not a similar schedule, create an event and add it to the dictionary
        dictionary_event = new UnityEvent();

        schedule_events.Add(schedule, dictionary_event);

        return dictionary_event;
    }

    private Days[] days_array = (Days[])Enum.GetValues(typeof(Days));

    public Days get_day(int day)
    {
        return days_array[day % days_array.Length];
    }

    private Dictionary<double, List<UnityEvent>> time_events = new Dictionary<double, List<UnityEvent>>();

    private void update_time_events_for_day(Days day)
    {
        time_events.Clear();

        foreach (KeyValuePair<Schedule, UnityEvent> entry in schedule_events)
        {
            List<Alarm> alarms = entry.Key.get_alarms();

            foreach (Alarm alarm in alarms)
            {
                if (!alarm.is_day_in_alarm(day))
                {
                    continue;
                }

                /// If the alarm sounds today
                List<UnityEvent> events_list;

                double alarm_time = alarm.get_alarm_time();

                /// If the alarm time was already in the dictionary
                if (time_events.TryGetValue(alarm_time, out events_list))
                {
                    events_list.Add(entry.Value);
                }
                /// If not, add the alarm
                else
                {
                    events_list = new List<UnityEvent>();
                    events_list.Add(entry.Value);

                    time_events[alarm_time] = events_list;
                }
            }
        }
    }

    /// List of sorted keys that holds the times when an alarm sounds
    private List<double> time_list;

    /// Variable to keep track of the day (its name is to represent it holds the previous frame day, not the previous day)
    private int prev_day = -1;

    private void time_events_notifier()
    {
        /// Get current hour and day
        int current_day = (int)Math.Truncate(current_time);
        double day_time = current_time - Math.Truncate(current_time);

        /// If a day has passed, reset 'time_events'
        if (current_day != prev_day)
        {
            update_time_events_for_day(get_day(current_day));
            time_list = time_events.Keys.ToList();
            time_list.Sort();

            current_date = current_date.AddDays(1);
        }

        /// If we still have events to be invoked for the day
        if (time_list.Count > 0)
        {
            /// If we reached a certain hour, make the event invocations
            if (time_list[0] >= day_time)
            {
                List<UnityEvent> events = time_events[time_list[0]];

                /// For some reason this foreach doesn't work
                /*foreach (UnityEvent event in events)
                {
                    //event.Invoke();
                }*/

                for (int i = 0; i < events.Count; ++i)
                {
                    events[i].Invoke();
                }

                time_list.RemoveAt(0);
            }
        }

        prev_day = current_day;
    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime * real_to_ingame_time_ratio;

        //print(current_time);

        time_events_notifier();
    }

}

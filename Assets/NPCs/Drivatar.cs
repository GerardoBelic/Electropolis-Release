using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drivatar : MonoBehaviour
{

    public GameObject car_gameobject = null;
    private NavMeshAgent car_agent = null;

    void Awake()
    {
        car_gameobject = gameObject;
        car_agent = gameObject.GetComponent<NavMeshAgent>();

        hide_gameobject_renderer();
    }

    public void hide_gameobject_renderer()
    {
        Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();

        foreach(Renderer r in rs)
        {
            r.enabled = false;
        }
    }

    public void show_gameobject_renderer()
    {
        Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();

        foreach(Renderer r in rs)
        {
            r.enabled = true;
        }
    }

    public Route avatar_route = null;

    public bool is_traveling = false;

    public static float wait_time_until_path_computes = 0.5f;
    public static float wait_time_until_target_reached = 2.0f;

    private Coroutine executing_route = null;
    public void start_route(Route route)
    {
        avatar_route = route;

        executing_route = StartCoroutine(consume_route());

        is_traveling = true;
    }

    private IEnumerator wait_until_path_computes(NavMeshAgent agent)
    {
        while (agent.pathPending)
        {
            yield return new WaitForSeconds(wait_time_until_path_computes);
        }
    }

    private IEnumerator wait_until_target_reached(NavMeshAgent agent)
    {
        while (agent.remainingDistance > 2.0f)
        {
            /// If the path is invalidated due to a road/building deletion, skip the path completition
            if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                yield break;
            }
            yield return new WaitForSeconds(wait_time_until_target_reached);
        }
    }

    private static System.Random random = new System.Random();
    private static float max_spawn_time = 5.0f;

    private IEnumerator wait_random_time()
    {
        float random_time = (float)random.NextDouble() * max_spawn_time;

        yield return new WaitForSeconds(random_time);
    }
    
    private IEnumerator consume_route()
    {
        yield return StartCoroutine(wait_random_time());
        
        /// We start spawning the human in the starting point
        car_gameobject.transform.position = avatar_route.get_starting_point().position;
        //car_gameobject.SetActive(true);
        show_gameobject_renderer();

        while (true)
        {
            (Route_Phase route_phase, Transform target) = avatar_route.consume_single_route_phase();

            /// If we have consumed all the route phases stop the coroutine
            if (route_phase == Route_Phase.Finished)
            {
                //car_gameobject.SetActive(false);
                hide_gameobject_renderer();

                /// Notify that we have reached our destination
                is_traveling = false;

                yield break;
            }

            /// If we haven't finished the route phases but a target is null, just transport the citizen to the building
            if (target == null)
            {
                //car_gameobject.SetActive(false);
                hide_gameobject_renderer();

                /// Notify that we have reached our destination
                is_traveling = false;

                yield break;
            }


            if (route_phase == Route_Phase.Going)
            {
                car_agent.SetDestination(target.position);

                yield return StartCoroutine(wait_until_path_computes(car_agent));

                yield return StartCoroutine(wait_until_target_reached(car_agent));

                continue;
            }
        }
    }

}

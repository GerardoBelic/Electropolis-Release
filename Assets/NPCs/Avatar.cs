using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Avatar : MonoBehaviour
{

    /*#region TEMPORAL

    public Transform _1;
    public Transform _2;
    public Transform _3;
    public Transform _4;
    public Transform _5;
    public Transform _6;
    public Building b;

    void Update()
    {
        if (_6 != null)
        {
            Route r = new Route(_1, _2, _3, _4, _5, _6, b);
            start_route(r);
            _1 = _2 = _3 = _4 = _5 = _6 = null;
            b = null;

        }
    }

    #endregion*/
    
    [HideInInspector] public GameObject human_gameobject = null;
    private NavMeshAgent human_agent = null;

    private Animator animator;

    void Awake()
    {
        human_gameobject = gameObject;
        human_agent = gameObject.GetComponent<NavMeshAgent>();

        animator = gameObject.GetComponent<Animator>();

        hide_gameobject_renderer();
    }

    void Update()
    {
        if (!is_traveling)
        {
            return;
        }

        animator.SetFloat("Speed", human_agent.velocity.magnitude);
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

    //public GameObject car_gameobject = null;
    //public NavMeshAgent car_agent = null;

    private Route avatar_route = null;

    public bool is_traveling = false;

    private static float wait_time_until_path_computes = 0.5f;
    private static float wait_time_until_target_reached = 1.0f;

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
        while (agent.remainingDistance > 0.75f)
        {
            //print("Distance: " + agent.remainingDistance);
            //print("isOnNavMesh: " + agent.isOnNavMesh);
            //print("isPathStale: " + agent.isPathStale);
            //print("pathStatus: " + agent.pathStatus);

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
        human_gameobject.transform.position = avatar_route.get_starting_point().position;
        //human_gameobject.SetActive(true);
        show_gameobject_renderer();

        while (true)
        {
            (Route_Phase route_phase, Transform target) = avatar_route.consume_single_route_phase();

            /// If we have consumed all the route phases stop the coroutine
            if (route_phase == Route_Phase.Finished)
            {
                //human_gameobject.SetActive(false);
                hide_gameobject_renderer();

                /// Notify that we have reached our destination
                is_traveling = false;

                yield break;
            }

            /// If we haven't finished the route phases but a target is null, just transport the citizen to the building
            if (target == null)
            {
                //human_gameobject.SetActive(false);
                hide_gameobject_renderer();

                /// Notify that we have reached our destination
                is_traveling = false;

                yield break;
            }


            if (route_phase == Route_Phase.Going)
            {
                human_agent.SetDestination(target.position);

                yield return StartCoroutine(wait_until_path_computes(human_agent));

                yield return StartCoroutine(wait_until_target_reached(human_agent));

                continue;
            }
            /*else if (route_phase == Route_Phase.Teleport_To_Walk)
            {
                //car_gameobject.SetActive(false);

                human_gameobject.transform.position = target.position;
                human_gameobject.SetActive(true);
                
                continue;
            }
            else if (route_phase == Route_Phase.Teleport_To_Drive)
            {
                human_gameobject.SetActive(false);

                //car_gameobject.transform.position = target.position;
                //ar_gameobject.SetActive(true);

                continue;
            }
            else if (route_phase == Route_Phase.Walk)
            {
                human_agent.SetDestination(target.position);

                yield return StartCoroutine(wait_until_path_computes(human_agent));

                yield return StartCoroutine(wait_until_target_reached(human_agent));

                continue;
            }
            else if (route_phase == Route_Phase.Drive)
            {
                //car_agent.SetDestination(target.position);

                //yield return StartCoroutine(wait_until_path_computes(car_agent));

                //yield return StartCoroutine(wait_until_target_reached(car_agent));

                continue;
            }*/
        }
    }

}

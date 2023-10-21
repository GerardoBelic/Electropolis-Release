using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPCDictionary : MonoBehaviour
{

    [SerializeField] private List<GameObject> avatar_prefabs;
    [SerializeField] private List<GameObject> drivatar_prefabs;
    
    private static float wait_time_until_rerouting = 10.0f;

    private List<Avatar> avatars = new List<Avatar>();
    private List<Drivatar> drivatars = new List<Drivatar>();

    void Start()
    {
        refill_npcs_and_reassign_routes_coroutine = StartCoroutine(refill_npcs_and_reassign_routes());
    }

    [SerializeField] private BuildingDictionary building_dictionary;
    [SerializeField] private RoadNetwork road_network;

    private Coroutine refill_npcs_and_reassign_routes_coroutine = null;
    private IEnumerator refill_npcs_and_reassign_routes()
    {
        while (true)
        {

            yield return StartCoroutine(refill_npcs());

            yield return StartCoroutine(reassign_routes());

            yield return new WaitForSeconds(wait_time_until_rerouting);
        }
    }

    private static System.Random random = new System.Random();

    private static float population_to_avatar_ratio = 0.2f;
    private static float job_to_drivatar_ratio = 0.2f;

    private IEnumerator refill_npcs()
    {        
        /// Avatars
        int avatar_count = (int)(building_dictionary.get_population_count() * population_to_avatar_ratio);

        /// Destroy avatars if the current avatar count is less than the new calculated one
        if (avatars.Count > avatar_count)
        {
            int avatars_to_delete = avatars.Count - avatar_count;

            for (int i = 0; i < avatars_to_delete; ++i)
            {
                Avatar avatar = avatars.Last();

                Destroy(avatar.human_gameobject);

                avatars.RemoveAt(avatars.Count - 1);
            }
        }
        /// Create avatars if the current avatar count is bigger than the new calculated one
        else if (avatars.Count < avatar_count)
        {
            int avatars_to_add = avatar_count - avatars.Count;

            /// This position is only to put the new agents in a navmesh, so it doesn't matter what position it is
            Transform initial_position = building_dictionary.get_random_building_position();

            for (int i = 0; i < avatars_to_add; ++i)
            {
                GameObject instance = Instantiate(avatar_prefabs[random.Next(avatar_prefabs.Count)], initial_position.position, Quaternion.identity);
                //instance.SetActive(false);
                instance.transform.parent = gameObject.transform;

                Avatar avatar = instance.GetComponent<Avatar>();

                avatars.Add(avatar);
            }
        }

        /// Drivatars
        //int drivatar_count = (int)(building_dictionary.get_job_count() * job_to_drivatar_ratio);
        int drivatar_count = (int)(building_dictionary.get_population_count() * job_to_drivatar_ratio);

        /// Destroy drivatars if the current drivatar count is less than the new calculated one
        if (drivatars.Count > drivatar_count)
        {
            int drivatars_to_delete = drivatars.Count - drivatar_count;

            for (int i = 0; i < drivatars_to_delete; ++i)
            {
                Drivatar drivatar = drivatars.Last();

                Destroy(drivatar.car_gameobject);

                drivatars.RemoveAt(drivatars.Count - 1);
            }
        }
        /// Create drivatars if the current drivatar count is bigger than the new calculated one
        else if (drivatars.Count < drivatar_count && road_network.at_least_one_road())
        {
            int drivatars_to_add = drivatar_count - drivatars.Count;

            /// This position is only to put the new agents in a navmesh, so it doesn't matter what position it is
            Transform initial_position = road_network.get_random_road_position();

            for (int i = 0; i < drivatars_to_add; ++i)
            {
                GameObject random_drivatar = drivatar_prefabs[random.Next(drivatar_prefabs.Count)];
                GameObject instance = Instantiate(random_drivatar, initial_position.position, Quaternion.identity);
                //instance.SetActive(false);
                instance.transform.parent = gameObject.transform;

                Drivatar drivatar = instance.GetComponent<Drivatar>();

                drivatars.Add(drivatar);
            }
        }

        yield break;
    }

    private IEnumerator reassign_routes()
    {
        foreach (Avatar avatar in avatars)
        {
            if (avatar.is_traveling)
            {
                continue;
            }

            Transform origin = building_dictionary.get_random_building_position();
            Transform target = building_dictionary.get_random_building_position();

            if (origin == target)
            {
                continue;
            }

            Route avatar_route = new Route(origin, target);

            avatar.start_route(avatar_route);
        }

        foreach (Drivatar drivatar in drivatars)
        {
            if (drivatar.is_traveling)
            {
                continue;
            }

            Transform origin = road_network.get_random_road_position();
            Transform target = road_network.get_random_road_position();

            if (origin == target)
            {
                continue;
            }

            Route drivatar_route = new Route(origin, target);

            drivatar.start_route(drivatar_route);
        }

        yield break;
    }

}

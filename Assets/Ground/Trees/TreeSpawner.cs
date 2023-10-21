using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private int trees_to_spawn = 20;
    [SerializeField] private float maximum_radius = 50.0f;

    private static System.Random random = new System.Random(0);

    public List<GameObject> get_tree_spawners()
    {
        List<GameObject> spawns = new List<GameObject>();

        for (int i = 0; i < trees_to_spawn; ++i)
        {
            float radius = (float)random.NextDouble();
            radius *= radius;
            radius = 1 - radius;
            radius *= maximum_radius;

            float angle = (float)random.NextDouble() * 360.0f;

            Vector3 offset = new Vector3(Mathf.Cos(angle) * radius, 0.0f, Mathf.Sin(angle) * radius);

            //GameObject spawn_instance = Instantiate(new GameObject("Spawn"), gameObject.transform.position + offset, Quaternion.identity);
            GameObject spawn_instance = new GameObject("Spawn");
            spawn_instance.transform.position = gameObject.transform.position + offset;
            spawn_instance.transform.parent = gameObject.transform;

            spawns.Add(spawn_instance);
        }

        return spawns;
    }
}

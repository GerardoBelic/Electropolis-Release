using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMiniComponent : MonoBehaviour
{
    [SerializeField] private Light light_component;

    void Start()
    {
        LightingManager lighting_manager = GameObject.Find("Light Manager").GetComponent<LightingManager>();

        light_component.intensity = 100.0f;
        light_component.intensity = 50.0f;

        lighting_manager.add_light(light_component);

        if (lighting_manager.should_turn_on_light())
        {
            light_component.enabled = true;
        }

        Destroy(this);
    }
}

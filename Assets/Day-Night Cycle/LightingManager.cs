using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light day_directional_light;
    [SerializeField] private Light night_directional_light;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private LightingPreset night_preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    [SerializeField] private TimeManager time_manager;


    private void Update()
    {
        /*if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }*/

        float day_time = (float)time_manager.get_current_time();

        if (day_time < 0.24f || day_time > 0.76f)
        {
            update_night_lighting(day_time);
        }
        else
        {
            update_day_lighting(day_time);
        }

        
    }

    private void update_night_lighting(float timePercent)
    {
        if (night_directional_light.enabled == false)
        {
            day_directional_light.enabled = false;
            night_directional_light.enabled = true;

            //day_directional_light.transform.localRotation = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));
            //night_directional_light.color = Preset.DirectionalColor.Evaluate(timePercent);

            //Set ambient and fog
            RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(0.0f);
            //RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

            turn_on_lights();
        }

        night_directional_light.color = night_preset.DirectionalColor.Evaluate(timePercent);

    }

    private void update_day_lighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        //RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (day_directional_light.enabled == false)
        {
            night_directional_light.enabled = false;
            day_directional_light.enabled = true;

            turn_off_lights();
        }

        day_directional_light.color = Preset.DirectionalColor.Evaluate(timePercent);

        day_directional_light.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, /*170f*/-225, 0));

    }

    private List<Light> lights = new List<Light>();

    public void add_light(Light light_component)
    {
        lights.Add(light_component);
    }

    public bool should_turn_on_light()
    {
        float day_time = (float)time_manager.get_current_time();

        if (day_time < 0.24f || day_time > 0.76f)
        {
            return true;
        }
        
        return false;
    }

    private void turn_on_lights()
    {
        lights.RemoveAll(item => item == null);
        foreach (Light light_component in lights)
        {
            light_component.enabled = true;
        }
    }

    private void turn_off_lights()
    {
        lights.RemoveAll(item => item == null);
        foreach (Light light_component in lights)
        {
            light_component.enabled = false;
        }
    }

    //Try to find a directional light to use if we haven't set one
    /*private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }*/
}
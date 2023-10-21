using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private static float max_size = 80.0f;
    private static float min_size = 4.0f;
    private static float step = 4.0f;

    [SerializeField] GameObject rain_fx;

    void Update()
    {
        if(Input.mouseScrollDelta.y > 0.5f && camera.orthographicSize > min_size)
        {
            camera.orthographicSize -= step;

            float rain_scale = camera.orthographicSize / 4.0f;
            rain_fx.transform.localScale = new Vector3(rain_scale, rain_scale, rain_scale);
        }
        else if(Input.mouseScrollDelta.y < -0.5f && camera.orthographicSize < max_size)
        {
            camera.orthographicSize += step;
            float rain_scale = camera.orthographicSize / 4.0f;
            rain_fx.transform.localScale = new Vector3(rain_scale, rain_scale, rain_scale);
        }
    }
    
}

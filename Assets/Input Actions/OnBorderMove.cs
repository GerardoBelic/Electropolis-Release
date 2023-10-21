using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBorderMove : MonoBehaviour
{

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private int pixels_from_border = 10;

    private bool border_move_enabled = true;

    public void enable_border_move()
    {
        border_move_enabled = true;
    }

    public void disable_border_move()
    {
        border_move_enabled = false;
    }

    /// If the mouse is on the border of the window, move the camera in that direction
    private void move_map_on_border()
    {
        int mouse_position_x = (int)Input.mousePosition.x;
        int mouse_position_y = (int)Input.mousePosition.y;

        Vector3 normal_position = new Vector3((float)mouse_position_x /Screen.width, 0.0f, (float)mouse_position_y /Screen.height);
        normal_position = Vector3.Normalize(normal_position) * 2.0f + new Vector3(-1.0f, -1.0f, -1.0f);

        /// Right edge
        if  (mouse_position_x >= Screen.width - pixels_from_border && mouse_position_x <= Screen.width)
        {
            // Move the camera
            transform.position += transform.right * Time.deltaTime * speed;
        }
        /// Left edge
        else if (mouse_position_x <= pixels_from_border && mouse_position_x >= 0)
        {
            transform.position += -transform.right * Time.deltaTime * speed;
        }

        /// Top edge
        if (mouse_position_y >= Screen.height - pixels_from_border && mouse_position_y <= Screen.height)
        {
            // Move the camera
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        /// Bottom edge
        else if (mouse_position_y <= pixels_from_border && mouse_position_y >= 0)
        {
            transform.position += -transform.forward * Time.deltaTime * speed;
        }

        //transform.position += (transform.forward * normal_position.z + transform.right * normal_position.x) * Time.deltaTime * speed;

    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (border_move_enabled)
        {
            move_map_on_border();
        }
    }
}

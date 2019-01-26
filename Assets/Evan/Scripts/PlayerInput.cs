using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{

    float horiz, vert;

    public static Vector2 GetMousePos ()
    {
        return Input.mousePosition;
    }

    public static float GetMouseX ()
    {
        return Input.GetAxisRaw("Mouse X");
    }

    public static float GetMouseY ()
    {
        return -Input.GetAxisRaw("Mouse Y");
    }
}

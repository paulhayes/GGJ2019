using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{

    float horiz, vert;

    public static bool GetLeftMouseDown ()
    {
        return Input.GetMouseButtonDown(0);
    }

    public static bool GetRightMouseDown ()
    {
        return Input.GetMouseButtonDown(1);
    }

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

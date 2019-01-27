using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] PauseView pauseView;

    bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (PlayerInput.GetPauseButtonDown())
        {
            pauseView.Toggle();
        }
    }

    public bool IsPaused ()
    {
        return pauseView.IsPaused();
    }
}

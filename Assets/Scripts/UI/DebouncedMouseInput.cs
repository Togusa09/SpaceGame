using System.Collections.Generic;
using UnityEngine;

public class DebouncedMouseInput
{
    private Dictionary<int, float> _mouseHoldStart = new Dictionary<int, float>
    {
        {0, 0 }, {1, 0}, {2, 0}
    };
    private float _mouseButtonDebounceInterval = 0.2f;

    public void Update()
    {
        var time = Time.time;
        for(var button = 0; button < 3; button++)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseHoldStart[button] = time;
            }

            if (Input.GetMouseButtonUp(1))
            {
                _mouseHoldStart[button] = 0;
            }
        }
    }

    // Time.time - _mouseHoldStart[button] > _mouseButtonDebounceInterval

    public bool GetMouseButtonDown(int buttonId)
    {
        return Input.GetMouseButtonDown(buttonId);
    }

    public bool GetMouseButtonHoldDown(int buttonId)
    {
        var time = Time.time;
        var mouseDown = Input.GetMouseButton(buttonId) || Input.GetMouseButtonUp(buttonId) || Input.GetMouseButtonDown(buttonId);
        if (mouseDown)
        {
            return time - _mouseHoldStart[buttonId] > _mouseButtonDebounceInterval;
        }

        return false;

        // If the mouse hold interval is longer than the  debounce interval
        
    }

    public bool GetMouseButtonUp(int buttonId)
    {
        // If the 
        return Input.GetMouseButtonUp(buttonId);
    }
}

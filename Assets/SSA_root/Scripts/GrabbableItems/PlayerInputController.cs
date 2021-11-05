using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputController : MonoBehaviour
{
    public event Action PressedRightSelect = delegate { };
    public event Action PressedLeftSelect = delegate { };
    public event Action PressedReload = delegate { };
   

    void Update()
    {
        InputActive();
    }

    void InputActive()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PressedRightSelect?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PressedLeftSelect?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PressedReload?.Invoke();
        }

    }



}

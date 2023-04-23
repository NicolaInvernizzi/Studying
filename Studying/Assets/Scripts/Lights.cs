using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : BaseEvent
{
    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
        del = ChangeIntensity;
    }
    private void ChangeIntensity()
    {
        _light.intensity += 5 * EventManager.eventSign;
    }
}

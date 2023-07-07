using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MapRule
{
    public Direction direction;
    public int[] constraints;
}

[Serializable]
public class ElementRule
{
    public Direction direction;
    public int[] constraints;

    public ElementRule (Direction direction)
    {
        this.direction = direction;
    }
}

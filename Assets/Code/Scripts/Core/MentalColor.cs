using System;
using Unity.VisualScripting;
using UnityEngine;


[Flags]
public enum MentalColorType
{
    None = 0,
    Red = 1,
    Blue = 2,
    Green = 4

    //next color must be 8, then 16, ...
}
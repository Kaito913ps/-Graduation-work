using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float targetHeight = 10.0f;

    public float GetTargetHeight()
    {
        return targetHeight;
    }
}

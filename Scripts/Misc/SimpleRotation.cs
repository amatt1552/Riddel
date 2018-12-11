using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// an attempt to make rotation actually make sense.
/// </summary>

public class SimpleRotation : MonoBehaviour
{
    public static SimpleRotation GC;
    private void Start()
    {
        GC = GetComponent<SimpleRotation>();
    }

    public Vector3 QuaternionToAngle()
    {
        return Vector3.zero;
    }
}

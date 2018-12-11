using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleOfVectorTest : MonoBehaviour
{
    
    public Transform posA;
    public Transform posB;
    void Update ()
    {

        
        Vector3 diff = posB.position - posA.position;
        
        print(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
	}
}

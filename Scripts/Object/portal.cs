using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{


	void Start ()
    {
		
	}
	
	
	void Update ()
    { 
        transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
	}
}

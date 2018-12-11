using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distort : MonoBehaviour
{
    public float speedX, speedY, speedZ;
    float x, y;
    

	void Start ()
    {
       
    }
	
	void Update ()
    {
        
        transform.Rotate(Vector3.up * speedY * Time.deltaTime);
        transform.Rotate(Vector3.right * speedX * Time.deltaTime);
        transform.Rotate(Vector3.forward * speedZ * Time.deltaTime);
       
    }

}

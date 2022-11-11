using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : MonoBehaviour
{
    public Transform target;

    public float speed = 0.1f;

    float range = 10.0f; 

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        float t = 1.0f - Steering.Attenuate(target.position, rb.position, range);
        Vector3 steeringForce = Vector3.zero;


        steeringForce = Steering.Seek(target.position + target.GetComponent<Rigidbody>().velocity, rb, speed);

        rb.AddForce(steeringForce);
    }

   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pursue : MonoBehaviour
{
    public Transform target;

    public float maxSpeed = 0.1f;

    float maxPrediction = 5.0f; 

    Rigidbody rb;
    public Rigidbody targetRb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRb = target.GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        
       

        Vector3 direction = target.position - transform.position;

        float distance = direction.magnitude;



        

        Vector3 futurePos = Steering.Seek(target.position + targetRb.velocity , rb , maxSpeed);

        rb.AddForce(futurePos);

        
    }

}

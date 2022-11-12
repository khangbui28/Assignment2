using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : MonoBehaviour
{

    public Transform target;

    public float speed = 0.1f;

    public float range = 10.0f;

    Rigidbody rb;
    Rigidbody targetRb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRb = target.GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= range)
        {

            Vector3 futurePos = Steering.Flee(target.position + targetRb.velocity, rb, speed);

            rb.AddForce(futurePos);
        }


       

    }
}

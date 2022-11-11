using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : MonoBehaviour
{

    public Transform target;

    public float speed = 0.1f;

    public float range = 10.0f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        float t = 1.0f - Steering.Attenuate(target.position, rb.position, range);
        Vector3 steeringForce = Vector3.zero;

        Vector3 a = -rb.velocity;
        Vector3 b = -Steering.Seek(target.position + target.GetComponent<Rigidbody>().velocity, rb, speed);
        steeringForce = Vector3.Lerp(a, b, t);

        rb.AddForce(steeringForce);
    }
}

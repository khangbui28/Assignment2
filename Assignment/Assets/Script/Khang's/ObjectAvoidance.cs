using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectAvoidance : MonoBehaviour
{
   
    float lookAhead = 2.0f;
    float sideAngle = 90f;

    Rigidbody rb;

    int rayAmount = 3 ;
    private void Start()
    {

        rb = GetComponent<Rigidbody>();
    }


  
}

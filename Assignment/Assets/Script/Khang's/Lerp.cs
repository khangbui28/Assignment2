using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
  
    public Transform dst;
    Vector3 startPos;

    float time = 0f;
    float timeMin = 0.0f ;
    public float timeMax= 100.0f ;

    public AnimationCurve curve; 
    private void Start()
    {
        startPos = transform.position;

        curve = GetComponent<AnimationCurve>();
    }

    private void Update()
    {

        time += Time.smoothDeltaTime;
        if (time > timeMax)
        {
            time = timeMin;
            
        }

        float t = time/timeMax ;


        transform.position = Vector3.Lerp(transform.position, dst.position, curve.Evaluate(t));
        //rb.velocity = Vector3.Lerp(src.position, dst.position, t);


    }
}

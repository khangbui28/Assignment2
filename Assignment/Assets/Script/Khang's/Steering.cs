using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering 
{
   public static Vector3 Seek(Vector3 target, Rigidbody current, float speed)
    {
        Vector3 targetDir = (target - current.position).normalized;
        Vector3 currentVel = current.velocity;
        Vector3 desiredVel = targetDir * speed - currentVel;
        return desiredVel;
    }

    public static Vector3 Flee(Vector3 target, Rigidbody current, float speed)
    {
        Vector3 targetDir = (target - current.position).normalized;
        Vector3 currentVel = current.velocity;
        Vector3 desiredVel = currentVel - targetDir * speed ;
        return desiredVel;
    }

}

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


	 
	public static float Attenuate(Vector3 target, Vector3 current, float length)
	{
		return Mathf.Clamp01((target - current).magnitude / length);
	}
}

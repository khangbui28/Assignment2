using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catmull : MonoBehaviour
{
	public Transform[] points;
	int i = 0;
	float t = 0.0f;

	

    private void Update()
    {
		Vector3 p0, p1, p2, p3;
		IndexPosition(i, points, out p0, out p1, out p2, out p3);

		

	   t += Time.smoothDeltaTime;
		if (t >= 1.0f)
        {
			t = 0.0f;
			i++;
        }

		transform.position = Hermite(p0, p1, p2, p3, t);
	}


    public static Vector3 Hermite(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{

		Vector3 r0 = -p0 + 3.0f * p1 - 3.0f * p2 + p3;
		Vector3 r1 = 2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3;
		Vector3 r2 = -p0 + p2;
		Vector3 r3 = 2.0f * p1;

		Vector3 p = 0.5f * (r3 + (r2 * t) + (r1 * t * t) + (r0 * t * t * t));
		return p;
	}

    private void OnDrawGizmos()
    {
        
    }
	public static void IndexPosition(int i, Transform[] points, out Vector3 p0, out Vector3 p1, out Vector3 p2, out Vector3 p3)
	{
		int n = points.Length;
		p0 = points[(i - 1 + n) % n].position;
		p1 = points[i % n].position;
		p2 = points[(i + 1) % n].position;
		p3 = points[(i + 2) % n].position;
	}

	

}

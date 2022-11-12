using System.Collections.Generic;
using UnityEngine;

public class CatmullSpeedControl : MonoBehaviour
{
    public Transform[] points;
    public float speed = 1f;
    [Range(1, 32)]
    public int sampleRate = 16;


    int maxIndex; 

    [System.Serializable]
    class SamplePoint
    {
        public float samplePosition;
        public float accumulatedDistance;

        public SamplePoint(float samplePosition, float distanceCovered)
        {
            this.samplePosition = samplePosition;
            this.accumulatedDistance = distanceCovered;
        }
    }
    //list of segment samples makes it easier to index later
    //imagine it like List<SegmentSamples>, and segment sample is a list of SamplePoints
    List<List<SamplePoint>> table = new List<List<SamplePoint>>();

    float distance = 0f;
    float accumDistance = 0f;
    int currentIndex = 0;
    int currentSample = 0;


    private void Start()
    {
        //make sure there are 4 points, else disable the component
        if (points.Length < 4)
        {
            enabled = false;
        }

        

        int size = points.Length;

        //calculate the speed graph table
        for (int i = 0; i < size; ++i)
        {
                List<SamplePoint> segment = new List<SamplePoint>();

            Vector3 p0, p1, p2, p3;

            IndexPosition(i, points, out p0, out p1, out p2, out p3);

            Vector3 previousPosition = Hermite(p0, p1, p2, p3, 0);

            segment.Add(new SamplePoint(0f, accumDistance));
            for (int sample = 1; sample <= sampleRate; ++sample)
            {

                float t = sample / sampleRate;
                Vector3 currentPosition = Hermite(p0, p1, p2, p3, t);

                Vector3 line = previousPosition - currentPosition;

                accumDistance += line.magnitude;

                previousPosition = currentPosition;
                segment.Add(new SamplePoint(t, accumDistance));
            }
            table.Add(segment);
        }
    }

    private void Update()
    {
        distance += speed * Time.deltaTime;

        //check if we need to update our samples
        while (distance > table[currentIndex][(currentSample + 1)].accumulatedDistance)
        {
            if (++currentSample >= sampleRate)
            {
                currentSample = 0;

                currentIndex++;
                

            }
            if (currentIndex >= points.Length)
            {

                distance -= accumDistance;
                currentIndex %= points.Length;  

            }

        }
       

        Vector3 p0, p1, p2, p3;

        IndexPosition(currentIndex, points, out p0, out p1, out p2, out p3);


        transform.position = Catmull(GetAdjustedT(), currentIndex, points);
    }

    float GetAdjustedT()
    {
        SamplePoint current = table[currentIndex][currentSample];
        SamplePoint next = table[currentIndex][(currentSample + 1)];

        return Mathf.Lerp(current.samplePosition, next.samplePosition,
            (distance - current.accumulatedDistance) / (next.accumulatedDistance - current.accumulatedDistance)
        // example:
        // distance = 6
        // current distance = 4
        // next distance = 8
        // distance - current distance = 6 - 4 = 2
        // next - current = 8 - 4 = 4
        // t = 2 / 4 = 0.5
        );
    }

    private void OnDrawGizmos()
    {
        Vector3 a, b, p0, p1, p2, p3;
        for (int i = 0; i < points.Length; i++)
        {
            a = points[i].position;
            p0 = points[(points.Length + i - 1) % points.Length].position;
            p1 = points[i].position;
            p2 = points[(i + 1) % points.Length].position;
            p3 = points[(i + 2) % points.Length].position;
            for (int j = 1; j <= sampleRate; ++j)
            {
                b = Hermite(p0, p1, p2, p3, (float)j / sampleRate);
                Gizmos.DrawLine(a, b);
                a = b;
            }
        }
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

    public static void IndexPosition(int i, Transform[] points, out Vector3 p0, out Vector3 p1, out Vector3 p2, out Vector3 p3)
    {
        int n = points.Length;
        p0 = points[(i - 1 + n) % n].position;
        p1 = points[i % n].position;
        p2 = points[(i + 1) % n].position;
        p3 = points[(i + 2) % n].position;
    }

    public static Vector3 Catmull(float t, int i, Transform[] points)
    {
        Vector3 p0, p1, p2, p3;
        IndexPosition(i, points, out p0, out p1, out p2, out p3);
        return Hermite(p0, p1, p2, p3, t);
    }

}

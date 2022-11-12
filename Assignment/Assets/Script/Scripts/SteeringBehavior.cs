using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour
{
    [SerializeField] protected float _maxSpeed, maxForce;
    protected float maxSpeed;

    protected Vector3 acceleration, velocity, location, startPosition;

    private Camera mainCamera;
    private float cameraOffSet;

    void Awake()
    {
        mainCamera = Camera.main;
        cameraOffSet = 2;
    }

    protected void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        location = transform.position;
        maxSpeed = _maxSpeed;
        startPosition = transform.position;
    }

    protected void ApplySteeringToMotion()
    {
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);

        location += velocity * Time.deltaTime;

        acceleration = Vector3.zero;

        RotateTowardTarget();


        transform.position = location;
    }

    protected virtual void Steer(Vector3 targetPosition)
    {

        Vector3 desiredVelocity = targetPosition - location;

        desiredVelocity.Normalize();


        desiredVelocity *= maxSpeed;



        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - velocity, maxForce);


        ApplyForce(steer);
    }

    protected void ApplyForce(Vector3 force)
    {

        acceleration += force;
    }

    protected void RotateTowardTarget()
    {

        Vector3 directionToDesiredLocation = location - transform.position;


        directionToDesiredLocation.Normalize();


        float rotZ = Mathf.Atan2(directionToDesiredLocation.y, directionToDesiredLocation.x) * Mathf.Rad2Deg;
        rotZ -= 90;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    protected virtual void WrapAroundCameraView()
    {


        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!onScreen)
        {

            if (screenPoint.y > 1) transform.position = new Vector3(transform.position.x, -mainCamera.orthographicSize + cameraOffSet, transform.position.z);
            /* Goes off the bottom edge of the screen */
            else if (screenPoint.y < 0) transform.position = new Vector3(transform.position.x, mainCamera.orthographicSize - cameraOffSet, transform.position.z);
            /* Goes off the right edge of the screen */
            else if (transform.position.x > 1) transform.position = new Vector3((-mainCamera.orthographicSize * mainCamera.aspect) + cameraOffSet, transform.position.y, transform.position.z);
            /* Goes off the left edge of the screen */
            else if (transform.position.x < 0) transform.position = new Vector3((mainCamera.orthographicSize * mainCamera.aspect) - cameraOffSet, transform.position.y, transform.position.z);

            /* Reset the location of the agent to the new location after it goes out of the view of the camera */
            location = transform.position;
            onScreen = false;
        }
    }
}

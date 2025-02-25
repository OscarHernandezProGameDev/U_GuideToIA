using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float speed = 2;
    public float distanceMin = 5;
    public float angleMin = 10;
    public float rotationSpeed = 0.2f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (CalculateDistance() < distanceMin)
            return;

        CalculateAngle();
        Move();
    }

    float CalculateDistance()
    {
        Vector3 fuelPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 tankPos = new Vector3(transform.position.x, 0, transform.position.z);
        float uDistance = Vector3.Distance(fuelPos, tankPos);

        return uDistance;
    }

    Vector3 Cross(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.x * w.z - v.z * w.x;
        float zMult = v.x * w.y - v.y * w.x;

        return (new Vector3(xMult, yMult, zMult));
    }

    void CalculateAngle()
    {
        Vector3 tankForward = transform.up;
        Vector3 fuelDirection = player.transform.position - transform.position;

        Debug.DrawRay(this.transform.position, tankForward * 10, Color.green, 5);
        Debug.DrawRay(this.transform.position, fuelDirection, Color.red, 5);

        float dot = tankForward.x * fuelDirection.x + tankForward.y * fuelDirection.y;
        float angle = Mathf.Acos(dot / (tankForward.magnitude * fuelDirection.magnitude));

        Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        Debug.Log("Unity Angle: " + Vector3.Angle(tankForward, fuelDirection));

        int clockwise = 1;
        if (Cross(tankForward, fuelDirection).z < 0)
            clockwise = -1;

        if (angle * Mathf.Rad2Deg > 10)
            this.transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockwise * rotationSpeed);
    }
    void Move()
    {
        Vector3 translation = transform.up * speed * Time.deltaTime;

        // Move translation along the object's z-axis
        //transform.Translate(translation);
        transform.position += translation;
    }
}

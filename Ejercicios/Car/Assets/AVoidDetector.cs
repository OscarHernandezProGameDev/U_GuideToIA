using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVoidDetector : MonoBehaviour
{
    public float avoidPath = 0;
    public float avoidTime = 0;
    public float wanderDistance = 4;
    public float avoidLength = 1;

    private void OnTriggerExit(Collider other)
    {
        if (gameObject.tag != "car")
            return;

        avoidTime = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject.tag != "car")
            return;

        Rigidbody otherCar = other.GetComponent<Rigidbody>();

        avoidTime = Time.time + avoidLength;

        Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
        float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z) * Mathf.Rad2Deg;

        avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle);
    }
}

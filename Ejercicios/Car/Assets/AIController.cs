using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Circuit circuit;
    public float steeringSensitivity = 0.01f;

    Drive[] ds;
    Vector3 target;
    Rigidbody rb;
    int currentWP = 0;

    // Start is called before the first frame update
    void Start()
    {
        ds = GetComponentsInChildren<Drive>();
        target = circuit.waypoints[currentWP].transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localTarget = transform.InverseTransformPoint(target);
        float distanceToTarget = Vector3.Distance(target, transform.position);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float a = 0.5f;
        float s = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(rb.velocity.magnitude);

        foreach (var d in ds)
            d.Go(a, s);

        if (distanceToTarget < 6)
        {
            currentWP++;
            if (currentWP >= circuit.waypoints.Length)
                currentWP = 0;
            target = circuit.waypoints[currentWP].transform.position;
        }
    }
}

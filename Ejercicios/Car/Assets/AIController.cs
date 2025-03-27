using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Circuit circuit;
    public float steeringSensitivity = 0.01f;
    public GameObject brakeLight;

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

        float a = 50f;
        float s = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(rb.velocity.magnitude);
        float b = 0;

        foreach (var d in ds)
            d.Go(a, s, b);

        if (b > 0)
        {
            brakeLight.SetActive(true);
        }
        else
        {
            brakeLight.SetActive(false);
        }

        if (distanceToTarget < 4)
        {
            currentWP++;
            if (currentWP >= circuit.waypoints.Length)
                currentWP = 0;
            target = circuit.waypoints[currentWP].transform.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public WheelCollider WC;
    public float torque = 200;
    public GameObject wheelMesh;
    public float maxSteerAngle = 30;
    public bool canTorn = false;

    // Start is called before the first frame update
    void Start()
    {
        WC = GetComponent<WheelCollider>();
    }

    void Go(float accel, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);

        float thrustTorque = accel * torque;

        WC.motorTorque = thrustTorque;
        if (canTorn)
        {
            steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
            WC.steerAngle = steer;
        }

        Quaternion quat;
        Vector3 position;

        WC.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }

    // Update is called once per frame
    void Update()
    {
        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");

        Go(a, s);
    }
}

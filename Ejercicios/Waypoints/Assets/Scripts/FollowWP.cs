using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class FollowWP : MonoBehaviour
{

    public GameObject[] waypoints;
    int currentWP = 0;

    public float speed = 10.0f;
    public float rotSpeed = 10.0f;
    public float lookAhead = 10.0f;

    GameObject tracker;

    void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<Renderer>().enabled = false;
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    void Update()
    {
        ProgessTracker();

        Quaternion lookAtWP = Quaternion.LookRotation(tracker.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, Time.deltaTime * rotSpeed);
        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
    }

    void ProgessTracker()
    {
        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead)
            return;

        if (Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 3.0f)
            currentWP++;

        if (currentWP >= waypoints.Length)
            currentWP = 0;

        tracker.transform.LookAt(waypoints[currentWP].transform);
        tracker.transform.Translate(0.0f, 0.0f, (speed + 20) * Time.deltaTime);
    }
}

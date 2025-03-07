using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public GameObject wpManager;

    Transform goal;
    public float speed = 5f;
    public float accuracy = 5f;
    public float rotSpeed = 2;

    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;
    Graph g;
    public void GoToHeli()
    {
        g.AStar(currentNode, wps[0]);
        currentWP = 0;
    }
    public void GoToRuin()
    {
        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];

        Invoke(nameof(GoToRuin), 2);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (g.pathList.Count == 0 || currentWP >= g.pathList.Count)
            return;

        currentNode = g.pathList[currentWP].GetId();

        if (Vector3.Distance(g.pathList[currentWP].GetId().transform.position, transform.position) < accuracy)
            currentWP++;

        if (currentWP < g.pathList.Count)
        {
            goal = g.pathList[currentWP].GetId().transform;

            Vector3 lookAtGoal = new Vector3(goal.transform.position.x, transform.position.y, goal.transform.position.z);
            Vector3 direction = lookAtGoal - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }
}

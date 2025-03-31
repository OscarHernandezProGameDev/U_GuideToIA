using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowWaypoints : MonoBehaviour
{
    public GameObject wpManager;

    GameObject[] wps;
    GameObject currentNode;
    NavMeshAgent agent;

    void Start()
    {
        Time.timeScale = 5.0f;
        wps = wpManager.GetComponent<WPManager>().waypoints;
        currentNode = wps[0];

        agent = GetComponent<NavMeshAgent>();

        // Invoke("GotoRuin", 2.0f);
    }

    public void GotoHeli()
    {
        //g.AStar(currentNode, wps[0]);
        agent.SetDestination(wps[0].transform.position);
    }

    public void GotoRuin()
    {
        //g.AStar(currentNode, wps[7]);
        agent.SetDestination(wps[7].transform.position);
    }

    public void GotoRock()
    {
        //g.AStar(currentNode, wps[1]);
        agent.SetDestination(wps[1].transform.position);
    }

    public void GotoFactory()
    {
        //g.AStar(currentNode, wps[4]);
        agent.SetDestination(wps[4].transform.position);
    }

    void LateUpdate()
    {


    }
}

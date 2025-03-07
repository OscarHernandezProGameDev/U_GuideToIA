using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public GameObject wpManager;

    Transform goal;
    float speed = 5f;
    float accuracy = 1f;
    float rotSpeed = 2;

    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;
    Graph g;
    public void GoToHeli()
    {
        g.AStar(currentNode, wps[0]);
        currentWP = 0;
    }
    public void GoToRuni()
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> edgeList = new List<Edge>();
    public Node path = null;
    public float xPos;
    public float yPos;
    public float zPos;

    public float f, g, h;
    public Node cameFrom;

    GameObject id;

    public Node(GameObject i)
    {
        id = i;
        xPos = i.transform.position.x;
        yPos = i.transform.position.y;
        zPos = i.transform.position.z;
        path = null;
    }

    public GameObject GetId() { return id; }
}

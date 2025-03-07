using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node starNode;
    public Node endNode;

    public Edge(Node from, Node to)
    {
        starNode = from;
        endNode = to;
    }
}

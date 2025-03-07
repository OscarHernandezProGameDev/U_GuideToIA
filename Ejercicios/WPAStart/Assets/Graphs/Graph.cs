using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    List<Node> pathList = new List<Node>();

    public void AddNode(GameObject id)
    {
        Node node = new Node(id);
        nodes.Add(node);
    }

    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        if (from != null && to != null)
        {
            Edge e = new Edge(from, to);
            from.edgeList.Add(e);
        }
    }

    public void ReconstructPath(Node startId, Node endId)
    {
        pathList.Clear();
        pathList.Add(endId);

        var p = endId.cameFrom;

        while (p != startId && p != null)
        {
            pathList.Insert(0, p);
            p = p.cameFrom;
        }
        pathList.Insert(0, startId);
    }

    private Node FindNode(GameObject id)
    {
        foreach (Node n in nodes)
        {
            if (n.GetId() == id)
                return n;
        }

        return null;
    }

    public bool AStar(GameObject startId, GameObject endId)
    {
        Node start = FindNode(startId);
        Node end = FindNode(endId);

        if (start == null || end == null)
            return false;

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentative_g_score = 0;
        bool tentative_is_better = false;

        start.g = 0;
        start.h = Distance(start, end);
        start.f = start.h;

        open.Add(start);
        while (open.Count > 0)
        {
            int i = lowestF(open);
            Node thisNode = open[i];

            if (thisNode.GetId() == endId)
            {
                ReconstructPath(start, end);

                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisNode);

            Node neightbour;

            foreach (Edge e in thisNode.edgeList)
            {
                neightbour = e.endNode;

                if (closed.IndexOf(neightbour) > -1)
                    continue;

                tentative_g_score = thisNode.g + Distance(thisNode, neightbour);
                if (open.IndexOf(neightbour) == -1)
                {
                    open.Add(neightbour);
                    tentative_is_better = true;
                }
                else if (tentative_g_score < neightbour.g)
                    tentative_is_better = true;
                else
                    tentative_is_better = false;

                if (tentative_is_better)
                {
                    neightbour.cameFrom = thisNode;
                    neightbour.g = tentative_g_score;
                    neightbour.h = Distance(neightbour, end);
                    neightbour.f = neightbour.g + neightbour.h;
                }
            }
        }

        return false;
    }

    float Distance(Node a, Node b)
    {
        return Vector3.SqrMagnitude(a.GetId().transform.position - b.GetId().transform.position);
    }

    int lowestF(List<Node> l)
    {
        float lowestf = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestf = l[0].f;

        for (int i = 1; i < l.Count; i++)
        {
            if (l[i].f < lowestf)
            {
                lowestf = l[i].f;
                iteratorCount = i;
            }
            count++;
        }

        return iteratorCount;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathMaker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject maker;
    public PathMaker parent;

    public PathMaker(MapLocation l, float g, float h, float f, GameObject maker, PathMaker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        this.maker = maker;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else
            return location.Equals(((PathMaker)obj).location);
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class FindPathAStar : MonoBehaviour
{
    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;
    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    List<PathMaker> open = new List<PathMaker>();
    List<PathMaker> closed = new List<PathMaker>();

    PathMaker goalNode;
    PathMaker startNode;
    PathMaker lastPos;
    bool done = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BeginSearch();
        }
    }
    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");

        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
    }
    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();

        for (int z = 1; z < maze.depth - 1; z++)
            for (int x = 1; x < maze.width - 1; x++)
            {
                if (maze.map[x, z] != 1)
                    locations.Add(new MapLocation(x, z));
            }

        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);

        startNode = new PathMaker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, Instantiate(start, startLocation, Quaternion.identity), null);

        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);

        goalNode = new PathMaker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0, Instantiate(end, goalLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();

        open.Add(startNode);
        lastPos = startNode;
    }

    void Search(PathMaker thisNode)
    {
        if (thisNode.Equals(goalNode))
        {
            done = true;

            return;
        }

        foreach (MapLocation dir in maze.directions)
        {
            MapLocation neighbour = dir + thisNode.location;

            if (maze.map[neighbour.x, neighbour.z] == 1)
                continue;

            if (neighbour.x < 1 || neighbour.x > maze.width || neighbour.z < 1 || neighbour.z > maze.depth)
                continue;

            if (IsClosed(neighbour))
                continue;

            // G = distancia desde la ubicación al punto a
            float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            // H= distancia entre el vecino y el objetivo
            float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float F = G + H;

            GameObject pathBlock= Instantiate(pathP, new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity)
        }
    }

    bool IsClosed(MapLocation marker)
    {
        foreach (PathMaker n in closed)
        {
            if (n.location.Equals(marker))
                return true;
        }

        return false;
    }
}

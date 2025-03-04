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

}

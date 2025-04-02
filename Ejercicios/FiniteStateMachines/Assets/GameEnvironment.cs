using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    private GameObject cubeSafe;
    public List<GameObject> Checkpoints { get => checkpoints; }
    public GameObject CubeSafe { get => cubeSafe; }
    public static GameEnvironment Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();
                instance.checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();
                instance.cubeSafe = GameObject.FindGameObjectWithTag("Safe");
            }

            return instance;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceQueue
{
    private Queue<GameObject> queue;
    public string tag;
    public string modSate;

    public ResourceQueue(string t, string ms, WorldStates w)
    {
        tag = t;
        modSate = ms;
        queue = new Queue<GameObject>();

        if (tag != "")
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject r in resources)
            {
                queue.Enqueue(r);
            }
        }
        if (modSate != "")
            w.ModifyState(modSate, queue.Count);
    }

    public void AddResource(GameObject r)
    {
        queue.Enqueue(r);
    }

    public void RemoveResource(GameObject r)
    {
        queue = new Queue<GameObject>(queue.Where(p => p != r));
    }

    public GameObject RemoveResource()
    {
        if (queue.Count == 0) return null;
        return queue.Dequeue();
    }
}

public sealed class GWorld
{
    // Our GWorld instance
    private static readonly GWorld instance = new GWorld();
    // Our world states
    private static WorldStates world;
    // Queue of patients
    private static ResourceQueue patients;
    // Queue of cubicles
    private static ResourceQueue cubicles;
    // Queue of offices
    private static ResourceQueue offices;
    private static ResourceQueue toilets;
    private static ResourceQueue puddles;
    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();
    static GWorld()
    {

        // Create our world
        world = new WorldStates();
        // Create patients array
        patients = new ResourceQueue("", "", world);
        resources.Add("patients", patients);
        // Create cubicles array
        cubicles = new ResourceQueue("Cubicle", "FreeCubicle", world);
        resources.Add("cubicles", cubicles);
        // Create offices array
        offices = new ResourceQueue("Office", "FreeOffice", world);
        resources.Add("offices", offices);
        // Create toilets array
        toilets = new ResourceQueue("Toilet", "FreeToilet", world);
        resources.Add("toilets", toilets);

        // Create puddles array
        puddles = new ResourceQueue("Puddle", "FreePuddle", world);
        resources.Add("puddles", puddles);

        // Set the time scale in Unity
        Time.timeScale = 5.0f;
    }

    private GWorld() { }

    public ResourceQueue GetQueue(string r)
    {
        return resources[r];
    }

    public static GWorld Instance
    {

        get { return instance; }
    }

    public WorldStates GetWorld()
    {

        return world;
    }
}

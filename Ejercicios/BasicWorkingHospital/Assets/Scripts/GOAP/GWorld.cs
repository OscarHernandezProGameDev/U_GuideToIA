using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{

    // Our GWorld instance
    private static readonly GWorld instance = new GWorld();
    // Our world states
    private static WorldStates world;
    // Queue of patients
    private static Queue<GameObject> patients;
    // Queue of cubicles
    private static Queue<GameObject> cubicles;
    // Queue of offices
    private static Queue<GameObject> offices;

    static GWorld()
    {

        // Create our world
        world = new WorldStates();
        // Create patients array
        patients = new Queue<GameObject>();
        // Create cubicles array
        cubicles = new Queue<GameObject>();
        // Create offices array
        offices = new Queue<GameObject>();

        // Find all GameObjects that are tagged "Cubicle"
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        // Then add them to the cubicles Queue
        foreach (GameObject c in cubes)
        {
            cubicles.Enqueue(c);
        }

        // Inform the state
        if (cubes.Length > 0)
        {
            world.ModifyState("FreeCubicle", cubes.Length);
        }

        // Find all GameObjects that are tagged "Office"
        GameObject[] offs = GameObject.FindGameObjectsWithTag("Office");
        // Then add them to the offices Queue
        foreach (GameObject o in offs)
        {
            offices.Enqueue(o);
        }

        // Inform the state
        if (offs.Length > 0)
        {
            world.ModifyState("FreeOffice", offs.Length);
        }

        // Set the time scale in Unity
        Time.timeScale = 5.0f;
    }

    private GWorld()
    {

    }

    // Add patient
    public void AddPatient(GameObject p)
    {

        // Add the patient to the patients Queue
        patients.Enqueue(p);
    }

    // Remove patient
    public GameObject RemovePatient()
    {

        if (patients.Count == 0) return null;
        return patients.Dequeue();
    }

    // Add cubicle
    public void AddCubicle(GameObject p)
    {

        // Add the patient to the patients Queue
        cubicles.Enqueue(p);
    }

    // Remove cubicle
    public GameObject RemoveCubicle()
    {

        // Check we have something to remove
        if (cubicles.Count == 0) return null;
        return cubicles.Dequeue();
    }

    // Add office
    public void AddOffice(GameObject p)
    {

        // Add the patient to the patients Queue
        offices.Enqueue(p);
    }

    // Remove office
    public GameObject RemoveOffice()
    {

        // Check we have something to remove
        if (offices.Count == 0) return null;
        return offices.Dequeue();
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

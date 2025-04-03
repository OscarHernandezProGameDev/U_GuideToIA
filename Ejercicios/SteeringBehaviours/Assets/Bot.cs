using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public GameObject target;

    NavMeshAgent agent;
    Drive ds;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ds = target.GetComponent<Drive>();
    }

    // Update is called once per frame
    void Update()
    {
        //Seek(target.transform.position);
        //Flee(target.transform.position);
        //Pursue();
        //Evade();
        //Wander();
        CleverHide();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

        if ((relativeHeading > 90 && toTarget < 20) || ds.currentSpeed < 0.01f)
        {
            Seek(target.transform.position);

            return;
        }

        float lookAHead = targetDir.magnitude / (agent.speed + ds.currentSpeed);

        Seek(target.transform.position + target.transform.forward * lookAHead);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float lookAHead = targetDir.magnitude / (agent.speed + ds.currentSpeed);

        Flee(target.transform.position + target.transform.forward * lookAHead);
    }

    Vector3 wanderTarget = Vector3.zero;

    void Wander()
    {
        float wanderRadius = 10f;
        float wanderDistance = 10f;
        float wanderJitter = 1f;

        wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformPoint(targetLocal);

        Seek(targetWorld);
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 choseSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                choseSpot = hidePos;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Seek(choseSpot);
    }

    void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            // 10 depende del ancho del objeto, si es muy grande puede pasar que el punto este dentro del objeto
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(transform.position, hidePos);
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 100.0f;

        hideCol.Raycast(backRay, out info, distance);

        Seek(info.point + chosenDir.normalized * 2);
    }
}

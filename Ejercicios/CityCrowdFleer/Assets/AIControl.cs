using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detectionRadius = 50f;
    float fleeRadius = 10f;

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) < detectionRadius)
        {
            Vector3 fleerDirection = (transform.position - position).normalized;
            Vector3 newgoal = transform.position + fleerDirection * fleeRadius;

            // no usamos SetDestination para evitar que el agente vaya a un punto fuera de la malla
            NavMeshPath path = new NavMeshPath();

            agent.CalculatePath(newgoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");

        int i = Random.Range(0, goalLocations.Length);

        agent.SetDestination(goalLocations[i].transform.position);

        anim = GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0f, 1f));

        ResetAgent();
    }
    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            ResetAgent();

            int i = Random.Range(0, goalLocations.Length);

            agent.SetDestination(goalLocations[i].transform.position);
        }
    }

    void ResetAgent()
    {
        speedMult = Random.Range(0.5f, 1.5f);

        anim.SetFloat("speedMult", speedMult);
        agent.speed *= speedMult;
        anim.SetTrigger("isWalking");
        agent.angularSpeed = 120;
        agent.ResetPath();
    }
}
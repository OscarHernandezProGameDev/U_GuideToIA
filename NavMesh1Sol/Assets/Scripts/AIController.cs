using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {

    public NavMeshAgent agent;
    public GameObject target;
    Animator anim;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //agent.speed = Random.Range(3.0f, 8.0f);
    }

    void Update() {

        agent.SetDestination(target.transform.position);
        if (agent.remainingDistance < 2.0f) {

            anim.SetBool("isMoving", false);
        } else {

            anim.SetBool("isMoving", true);
        }
    }
}

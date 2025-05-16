using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    public enum ActionState { IDLE, WORKING };

    protected BehaviourTree tree;
    protected NavMeshAgent agent;
    protected ActionState state = ActionState.IDLE;
    protected Node.Status treeStatus = Node.Status.RUNNING;

    WaitForSeconds waitForSeconds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1f));
        StartCoroutine(Behave());
    }

    IEnumerator Behave()
    {
        while (true)
        {
            tree.Process();
            yield return waitForSeconds;
        }
    }

    protected Node.Status GoToLocation(Vector3 destination)
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }
}

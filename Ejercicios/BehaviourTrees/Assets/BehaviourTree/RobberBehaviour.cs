using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    public GameObject diamond;
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;

    BehaviourTree tree;
    NavMeshAgent agent;
    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go to Diamond", GoToDiamond);
        Leaf goToBackDoor = new Leaf("Go to Backdoor", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go to Frontdoor", GoToFrontDoor);
        Leaf goToVan = new Leaf("Go to Van", GoToVan);
        Selector opendoor = new Selector("Open Door");

        opendoor.AddChild(goToBackDoor);
        opendoor.AddChild(goToFrontDoor);

        steal.AddChild(opendoor);
        steal.AddChild(goToDiamond);
        //steal.AddChild(goToBackDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        //Example for printTree
        /*
                Node eat = new Node("Eat Something");
                Node pizza = new Node("Go To Pizza");
                Node buy = new Node("Buy Pizza");

                eat.AddChild(pizza);
                eat.AddChild(buy);
                tree.AddChild(eat);
                */

        tree.PrintTree();
    }

    public Node.Status GoToDiamond()
    {
        return GoToLocation(diamond.transform.position);
    }

    private Node.Status GoToBackDoor()
    {
        return GoToLocation(backdoor.transform.position);
    }

    private Node.Status GoToFrontDoor()
    {
        return GoToLocation(frontdoor.transform.position);
    }

    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distanteToTarget = Vector3.Distance(destination, transform.position);

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
        else if (distanteToTarget < 2)
        {
            state = ActionState.IDLE;

            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    // Update is called once per frame
    void Update()
    {
        if (treeStatus == Node.Status.RUNNING)
            treeStatus = tree.Process();
    }
}

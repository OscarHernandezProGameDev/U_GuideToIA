using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    public GameObject diamond;
    public GameObject painting;
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;
    public GameObject cop;

    public GameObject[] arts;

    [Range(0, 1000)]
    public int money = 800;

    GameObject pickup;
    Leaf goToBackDoor;
    Leaf goToFrontDoor;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 1);
        Leaf goToPainting = new Leaf("Go To Painting", GoToPainting, 2);
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);

        RSelector selectObject = new RSelector("Select Object To Steal");

        for (int i = 0; i < arts.Length; i++)
        {

            Leaf gta = new($"Go To Art {arts[i].name}", i, GoToArt);

            selectObject.AddChild(gta);
        }

        goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor, 1);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        PSelector opendoor = new PSelector("Open Door");

        Sequence runAway = new Sequence("Run Away");
        Leaf canSeeCop = new Leaf("Can See Cop?", CanSeeCop);
        Leaf fleeFromCop = new Leaf("Flee From Cop", FleeFromCop);

        Inverter inverterMoney = new Inverter("Has Money");
        inverterMoney.AddChild(hasGotMoney);

        opendoor.AddChild(goToFrontDoor);
        opendoor.AddChild(goToBackDoor);

        runAway.AddChild(canSeeCop);
        runAway.AddChild(fleeFromCop);

        Inverter cantSeeCop = new Inverter("Can't See Cop");
        cantSeeCop.AddChild(canSeeCop);

        Sequence s1 = new Sequence("s1");
        s1.AddChild(inverterMoney);

        Sequence s2 = new Sequence("s2");
        s2.AddChild(cantSeeCop);
        s2.AddChild(opendoor);

        Sequence s3 = new Sequence("s3");
        s3.AddChild(cantSeeCop);
        s3.AddChild(selectObject);

        Sequence s4 = new Sequence("s4");
        s4.AddChild(inverterMoney);
        s4.AddChild(goToVan);


        /*
        steal.AddChild(s1);
        steal.AddChild(s2);
        steal.AddChild(s3);
        steal.AddChild(s4);
        */

        BehaviourTree seeCop = new BehaviourTree();
        seeCop.AddChild(cantSeeCop);

        DepSequence steal = new DepSequence("Steal Something", seeCop, agent);

        steal.AddChild(inverterMoney);
        steal.AddChild(opendoor);
        steal.AddChild(selectObject);
        steal.AddChild(goToVan);

        Selector beThief = new Selector("Be a Thief");

        beThief.AddChild(steal);
        beThief.AddChild(runAway);

        tree.AddChild(beThief);

        tree.PrintTree();

    }

    public Node.Status CanSeeCop()
    {
        return CanSee(cop.transform.position, "Cop", 50, 90);
    }

    public Node.Status FleeFromCop()
    {
        return Flee(cop.transform.position, 10);
    }

    public Node.Status HasMoney()
    {
        if (money < 500)
            return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    public Node.Status GoToDiamond()
    {
        if (!diamond.activeSelf)
            return Node.Status.FAILURE;

        Node.Status s = GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            diamond.transform.parent = this.gameObject.transform;
            pickup = diamond;
        }
        return s;
    }

    public Node.Status GoToPainting()
    {
        if (!painting.activeSelf)
            return Node.Status.FAILURE;

        Node.Status s = GoToLocation(painting.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            painting.transform.parent = this.gameObject.transform;
            pickup = painting;
        }
        return s;
    }

    public Node.Status GoToArt(int i)
    {
        if (!arts[i].activeSelf)
            return Node.Status.FAILURE;

        Node.Status s = GoToLocation(arts[i].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            arts[i].transform.parent = this.gameObject.transform;
            pickup = arts[i];
        }
        return s;
    }

    public Node.Status GoToBackDoor()
    {
        Node.Status s = GoToDoor(backdoor);

        if (s == Node.Status.FAILURE)
            goToBackDoor.sortOrder = 10;
        else
            goToBackDoor.sortOrder = 1;

        return s;
    }

    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(frontdoor);

        if (s == Node.Status.FAILURE)
            goToFrontDoor.sortOrder = 10;
        else
            goToFrontDoor.sortOrder = 1;

        return s;
    }

    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            money += 300;
            pickup.SetActive(false);
        }
        return s;
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }
}

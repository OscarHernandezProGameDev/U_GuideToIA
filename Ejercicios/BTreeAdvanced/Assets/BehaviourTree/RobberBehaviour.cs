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
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 1);
        Leaf goToPainting = new Leaf("Go To Painting", GoToPainting, 2);
        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);

        Leaf goToArt1 = new Leaf("Go To Art 1", GoToArt1);
        Leaf goToArt2 = new Leaf("Go To Art 2", GoToArt2);
        Leaf goToArt3 = new Leaf("Go To Art 3", GoToArt3);
        //Leaf goToArt4 = new Leaf("Go To Painting", GoToArt4);
        //Leaf goToArt5 = new Leaf("Go To Painting", GoToArt5);
        //Leaf goToArt6 = new Leaf("Go To Painting", GoToArt6);
        //Leaf goToArt7 = new Leaf("Go To Painting", GoToArt7);
        //Leaf goToArt8 = new Leaf("Go To Painting", GoToArt8);
        //Leaf goToArt9 = new Leaf("Go To Painting", GoToArt9);
        //Leaf goToArt10 = new Leaf("Go To Painting", GoToArt10);

        goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor, 1);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        PSelector opendoor = new PSelector("Open Door");
        RSelector selectObject = new RSelector("Select Object To Steal");

        Inverter inverterMoney = new Inverter("Has Money");
        inverterMoney.AddChild(hasGotMoney);

        opendoor.AddChild(goToFrontDoor);
        opendoor.AddChild(goToBackDoor);

        steal.AddChild(inverterMoney);
        steal.AddChild(opendoor);

        selectObject.AddChild(goToArt1);
        selectObject.AddChild(goToArt2);
        selectObject.AddChild(goToArt3);

        steal.AddChild(selectObject);

        //steal.AddChild(goToBackDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);

        tree.PrintTree();

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

    public Node.Status GoToArt1()
    {
        if (!arts[0].activeSelf)
            return Node.Status.FAILURE;

        Node.Status s = GoToLocation(arts[0].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            arts[0].transform.parent = this.gameObject.transform;
            pickup = arts[0];
        }
        return s;
    }

    public Node.Status GoToArt2()
    {
        if (!arts[1].activeSelf)
            return Node.Status.FAILURE;

        Node.Status s = GoToLocation(arts[1].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            arts[1].transform.parent = this.gameObject.transform;
            pickup = arts[1];
        }
        return s;
    }

    public Node.Status GoToArt3()
    {
        if (!arts[2].activeSelf)
            return Node.Status.FAILURE;

        Node.Status s = GoToLocation(arts[2].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            arts[2].transform.parent = this.gameObject.transform;
            pickup = arts[2];
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

using UnityEngine;

public class Worker : BTAgent
{
    public GameObject office;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        Leaf goToPatron = new Leaf("Go To Patron", GoToPatron);
        Leaf goToOffice = new Leaf("Go To Office", GoToOffice);

        Selector beWorker = new Selector("Be a Worker");
        beWorker.AddChild(goToPatron);
        beWorker.AddChild(goToOffice);

        tree.AddChild(beWorker);
    }

    public Node.Status GoToPatron()
    {
        if (Blackboard.Instance.patron == null)
            return Node.Status.FAILURE;

        Node.Status s = GoToLocation(Blackboard.Instance.patron.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            Blackboard.Instance.patron.GetComponent<PatronBehaviour>().ticket = true;
            Blackboard.Instance.DeregisterPatron();
        }

        return s;
    }

    public Node.Status GoToOffice()
    {
        Node.Status s = GoToLocation(office.transform.position);

        return s;
    }
}
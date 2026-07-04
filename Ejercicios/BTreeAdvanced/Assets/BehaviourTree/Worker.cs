using UnityEngine;

public class Worker : BTAgent
{
    public GameObject office;

    private GameObject patron;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        Leaf allocatePatron = new Leaf("Allocate Patron", AllocatePatron);
        Leaf goToPatron = new Leaf("Go To Patron", GoToPatron);
        Leaf goToOffice = new Leaf("Go To Office", GoToOffice);

        Sequence getPatron = new Sequence("Find a patron");
        getPatron.AddChild(allocatePatron);
        getPatron.AddChild(goToPatron);

        Selector beWorker = new Selector("Be a Worker");
        beWorker.AddChild(getPatron);
        beWorker.AddChild(goToOffice);

        tree.AddChild(beWorker);
    }

    // public Node.Status PatronWaiting()
    // {
    //     
    // }

    public Node.Status AllocatePatron()
    {
        if (Blackboard.Instance.patrons.Count == 0)
            return Node.Status.FAILURE;

        patron = Blackboard.Instance.patrons.Pop();
        if (patron == null)
            return Node.Status.FAILURE;

        return Node.Status.SUCCESS;
    }

    public Node.Status GoToPatron()
    {
        if (patron == null)
            return Node.Status.FAILURE;
        Node.Status s = GoToLocation(patron.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            patron.GetComponent<PatronBehaviour>().ticket = true;
            patron = null;
        }

        return s;
    }

    public Node.Status GoToOffice()
    {
        Node.Status s = GoToLocation(office.transform.position);
        patron = null;
        return s;
    }
}
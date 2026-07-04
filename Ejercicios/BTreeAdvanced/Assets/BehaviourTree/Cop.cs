using UnityEngine;

public class Cop : BTAgent
{
    public GameObject[] patrolPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        Sequence selectPatrolPoint = new Sequence("Select Patrol Point");
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf pp = new Leaf("Go to " + patrolPoints[i].name, i, GoToPatrolPoint);
            selectPatrolPoint.AddChild(pp);
        }

        tree.AddChild(selectPatrolPoint);
    }

    public Node.Status GoToPatrolPoint(int i)
    {
        Node.Status s = GoToLocation(patrolPoints[i].transform.position);
        return s;
    }
}
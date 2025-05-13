using UnityEngine;

public class Patient : GAgent
{
    protected override void Start()
    {

        // Call the base start
        base.Start();
        // Set up the subgoal "isWaiting"
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        // Add it to the goals
        goals.Add(s1, 3);

        // Set up the subgoal "isTreated"
        SubGoal s2 = new SubGoal("isTreated", 1, true);
        // Add it to the goals
        goals.Add(s2, 5);

        // Set up the subgoal "isHome"
        SubGoal s3 = new SubGoal("isHome", 1, true);
        // Add it to the goals
        goals.Add(s3, 1);

        // Refief goal
        SubGoal s4 = new SubGoal("refief", 1, false);
        goals.Add(s4, 2);

        // Call the NeedRefief() method for the first time
        Invoke("NeedRefief", Random.Range(2, 5));
    }

    void NeedRefief()
    {

        beliefs.ModifyState("busting", 0);
        //call the get tired method over and over at random times to make the nurse
        //get tired again
        Invoke("NeedRefief", Random.Range(2, 5));
    }
}
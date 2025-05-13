using UnityEngine;

public class Doctor : GAgent
{
    protected override void Start()
    {
        // Call base Start method
        base.Start();
        // Set goal so that it can't be removed so the nurse can repeat this action
        SubGoal s1 = new SubGoal("research", 1, false);
        goals.Add(s1, 1);

        // Refief goal
        SubGoal s2 = new SubGoal("refief", 1, false);
        goals.Add(s2, 2);

        // Resting goal
        SubGoal s3 = new SubGoal("rested", 1, false);
        goals.Add(s3, 3);

        // Call the GetTired() method for the first time
        Invoke("GetTired", Random.Range(2, 5));

        // Call the NeedRefief() method for the first time
        Invoke("NeedRefief", Random.Range(2, 5));
    }

    void GetTired()
    {

        beliefs.ModifyState("exhausted", 0);
        //call the get tired method over and over at random times to make the nurse
        //get tired again
        Invoke("GetTired", Random.Range(2, 5));
    }

    void NeedRefief()
    {

        beliefs.ModifyState("busting", 0);
        //call the get tired method over and over at random times to make the nurse
        //get tired again
        Invoke("NeedRefief", Random.Range(2, 5));
    }
}

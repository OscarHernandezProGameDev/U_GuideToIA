# Extra: Attacking and Chasing Framework

A number of students have asked about adding a chase and attack mechanic to the GOAP system.  While there isn't the need for such actions in a hospital simulation it's still a plausible scenario in any game environment.  So here are some pointers on getting started for yourself.

To implement chase and attack mechanics in the GOAP (Goal Oriented Action Planning) system, here's how you can structure the system with the files you've uploaded. The chase and attack actions will be added as `GAction` subclasses that handle the specific goals and world states related to chasing and attacking an enemy.

Steps to Implement Chase and Attack Mechanics:

1. Create Chase and Attack Actions:

You need to subclass `GAction` to create actions for chasing and attacking.

```C#
public class ChaseAction : GAction
{
    public override bool PrePerform()
    {
        // Add conditions that must be met to start chasing.
        return true;
    }
    public override bool PostPerform()
    {
        // Update the world states when chase is complete or interrupted.
        return true;
    }
}


public class AttackAction : GAction
{
    public override bool PrePerform()
    {
        // Conditions to start attacking (e.g., is in range of target).
        return true;
    }
 
    public override bool PostPerform()
    {
        // Handle updating world states after attack action is completed.
        return true;
    }
}
```

2. Define Preconditions and Effects:

Each action should have its preconditions and effects set, which influence the planning process.

For example, for ChaseAction, the precondition might be that the enemy is visible, and the effect might be that the agent is close to the enemy.

For AttackAction, the precondition might be that the agent is within attack range, and the effect might be that the enemy is damaged.

```C#
public ChaseAction()
{
    // Define preconditions and effects for chase
    AddPrecondition("enemyVisible", true); // Must see the enemy to chase
    AddEffect("enemyInRange", true); // The goal is to get in range of the enemy
}
 
public AttackAction()
{
    // Define preconditions and effects for attack
    AddPrecondition("enemyInRange", true); // Must be in range to attack
    AddEffect("enemyDamaged", true); // The goal is to damage the enemy
}
```

3. Create a Goal for the Agent:

In `GAgent`, the goal should be set to defeat or damage the enemy, which will trigger the chase and attack behavior. You will use `WorldStates` to determine whether the agent should switch between chasing and attacking.

```C#
public class EnemyAgent : GAgent
{
    protected override void Start()
    {
        base.Start();
        // Define the goal of defeating the enemy
        SubGoal goal = new SubGoal("enemyDamaged", 1, false);
        goals.Add(goal, 1);
    }
}
```

4. Monitor World States:

In `GWorld`, ensure that world states like `enemyVisible` and `enemyInRange` are properly managed. These will act as triggers for actions.

```C#
public class GStateMonitor : MonoBehaviour
{
    void Update()
    {
        // Example of updating world state based on whether enemy is visible or not
        bool enemyVisible = CheckIfEnemyVisible();
        GWorld.Instance.GetWorld().ModifyState("enemyVisible", enemyVisible ? 1 : 0);
    }
 
    bool CheckIfEnemyVisible()
    {
        // Logic for checking if the enemy is visible
        return true; // Example return value
    }
}
```

5. Plan Execution:

`GPlanner` will decide when to execute the chase and attack actions based on the world states. Ensure that the planner correctly picks actions that fulfill the goal of damaging the enemy.

```C#
public void ExecutePlan()
{
    // Get the plan from GPlanner
    Queue<GAction> actions = planner.Plan();
 
    // Process actions (chase, attack, etc.)
    while (actions.Count > 0)
    {
        GAction action = actions.Dequeue();
        action.Perform();
    }
}
```

Summary of Key Elements:

- ChaseAction: Moves the agent toward the enemy.

- AttackAction: Damages the enemy when in range.

- Preconditions/Effects: Help the planner decide the correct sequence (chase → attack).

- WorldStates: Used to track conditions like `enemyVisible` and `enemyInRange`.

- Goal: Set the agent's goal as damaging or defeating the enemy.

This framework will allow the GOAP system to dynamically plan when to chase and attack based on the current world state and the agent's goals.
using UnityEngine;

public class CleanUpPuddle : GAction
{
    public override bool PostPerform()
    {
        target = GWorld.Instance.GetQueue("puddles").RemoveResource();
        if (target == null)
            return false;

        inventory.AddItem(target);
        GWorld.Instance.GetWorld().ModifyState("FreePuddle", -1);

        return true;
    }

    public override bool PrePerform()
    {
        inventory.RemoveItem(target);
        Destroy(target);
        beliefs.RemoveState("clean");
        return true;
    }
}

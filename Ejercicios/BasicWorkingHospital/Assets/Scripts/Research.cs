using UnityEngine;

public class Research : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemoveOffice();
        if (target == null)
            return false;
        inventory.AddItem(target);
        GWorld.Instance.GetWorld().ModifyState("FreeOffice", -1);
        Debug.Log("Research Started");
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.AddOffice(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("FreeOffice", 1);
        Debug.Log("Research Finished");
        return true;
    }
}

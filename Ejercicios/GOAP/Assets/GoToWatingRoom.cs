using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWatingRoom : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Waiting", 1);
        GWorld.Instance.AddPatient(gameObject);
        return true;
    }
}

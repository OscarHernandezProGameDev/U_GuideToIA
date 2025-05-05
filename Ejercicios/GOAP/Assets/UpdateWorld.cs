using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour
{
    public Text states;

    void LateUpdate()
    {
        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
        states.text = "";
        foreach (KeyValuePair<string, int> kvp in worldStates)
        {
            states.text += kvp.Key + ", " + kvp.Value + "\n";
        }
    }
}

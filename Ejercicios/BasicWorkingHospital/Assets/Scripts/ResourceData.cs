using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "ResourceData", order = 51)]
public class ResourceData : ScriptableObject
{
    public string resourceTag;
    public string resourceQueue;
    public string resourceState;
}

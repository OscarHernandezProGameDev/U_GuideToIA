using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Winterface : MonoBehaviour
{
    public GameObject newResourcePrefab;
    public GameObject hospital;
    public NavMeshSurface surface;

    GameObject focusObj;
    Vector3 goalPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit))
                return;

            goalPos = hit.point;

            focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
        }
        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            focusObj.transform.parent = hospital.transform;
            surface.BuildNavMesh();
            GWorld.Instance.GetQueue("toilets").AddResource(focusObj);
            GWorld.Instance.GetWorld().ModifyState("FreeToilet", 1);
            focusObj = null;
        }
        else if (focusObj && Input.GetMouseButton(0))
        {
            RaycastHit hitMove;
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(rayMove, out hitMove))
                return;

            goalPos = hitMove.point;
            focusObj.transform.position = goalPos;
        }
    }
}

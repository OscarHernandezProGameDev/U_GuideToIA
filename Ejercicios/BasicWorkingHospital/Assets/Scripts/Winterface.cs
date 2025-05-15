using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Winterface : MonoBehaviour
{
    public GameObject newResourcePrefab;
    public GameObject hospital;
    public NavMeshSurface surface;

    GameObject focusObj;
    ResourceData foData;
    Vector3 goalPos;
    Vector3 clickOffset = Vector3.zero;
    bool offsetCalc = false;
    bool deleteResource = false;

    public void MouseOnHoverTrash()
    {
        deleteResource = true;
    }

    public void MouseOutHoverTrash()
    {
        deleteResource = false;
    }

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

            offsetCalc = false;
            clickOffset = Vector3.zero;

            Resource r = hit.transform.GetComponent<Resource>();

            if (r != null)
            {
                focusObj = hit.transform.gameObject;
                foData = r.info;
            }
            else
            {
                goalPos = hit.point;

                focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
            }

            focusObj.GetComponent<Collider>().enabled = false;
        }
        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            if (deleteResource)
            {
                GWorld.Instance.GetQueue(foData.resourceQueue).RemoveResource(focusObj);
                GWorld.Instance.GetWorld().ModifyState(foData.resourceState, -1);
                Destroy(focusObj);
            }
            else
            {
                focusObj.transform.parent = hospital.transform;

                GWorld.Instance.GetQueue(foData.resourceQueue).AddResource(focusObj);
                GWorld.Instance.GetWorld().ModifyState(foData.resourceState, 1);

                focusObj.GetComponent<Collider>().enabled = true;
            }

            surface.BuildNavMesh();

            focusObj = null;
        }
        else if (focusObj && Input.GetMouseButton(0))
        {
            RaycastHit hitMove;
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(rayMove, out hitMove))
                return;

            if (!offsetCalc)
            {
                clickOffset = hitMove.point - focusObj.transform.position;
                offsetCalc = true;
            }

            goalPos = hitMove.point - clickOffset;
            focusObj.transform.position = goalPos;
        }

        if (focusObj && (Input.GetKeyDown(KeyCode.Less) || Input.GetKeyDown(KeyCode.Comma)))
            focusObj.transform.Rotate(0, 90, 0);
        else if (focusObj && (Input.GetKeyDown(KeyCode.Greater) || Input.GetKeyDown(KeyCode.Period)))
            focusObj.transform.Rotate(0, -90, 0);
    }
}

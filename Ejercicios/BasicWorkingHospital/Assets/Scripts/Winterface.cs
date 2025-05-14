using UnityEngine;

public class Winterface : MonoBehaviour
{
    public GameObject newResourcePrefab;
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

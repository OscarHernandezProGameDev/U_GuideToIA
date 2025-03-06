using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public GameObject[] waypoints;
    public float speed = 10f;
    public float rotSpeed = 10f;

    int currentWP = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // El umbra hay que tener cuidado porque si la velocidad de giro es pequeña puede pasar que no llegue a la posición
        // por lo que se puede quedar en un bucle infinito, en ese caso se puede aumentar el umbral o la velocidad de giro
        if (Vector3.Distance(transform.position, waypoints[currentWP].transform.position) < 3)
            currentWP++;

        if (currentWP >= waypoints.Length)
            currentWP = 0;

        //transform.LookAt(waypoints[currentWP].transform);

        Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWP].transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}

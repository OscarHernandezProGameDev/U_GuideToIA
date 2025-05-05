using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numberOfPatients;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < numberOfPatients; i++)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
        }

        Invoke("SpawnPatient", 5f);
    }

    private void SpawnPatient()
    {
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke("SpawnPatient", Random.Range(2f, 10f));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

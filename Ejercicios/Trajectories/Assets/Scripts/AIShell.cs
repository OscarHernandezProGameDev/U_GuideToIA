using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShell : MonoBehaviour
{
    public GameObject explosion;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Para orientar la bala segun la dirección que tiene
        transform.forward = rb.velocity;
    }
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "tank")
        {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }
}

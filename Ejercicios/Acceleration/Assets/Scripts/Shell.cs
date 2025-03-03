using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject explosion;
    float speed = 0f;
    float yspeed = 0f;
    float mass = 10;
    float force = 1;
    float drag = 1;
    float gravity = -9.8f;
    float gAccel;
    float acceleration;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "tank")
        {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // La aceleración se aplica al inicio
        acceleration = force / mass;
        speed += acceleration;
        gAccel = gravity / mass;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        // La acceleración se aplicada cada frame
        //acceleration = force / mass;
        //speed += acceleration;
        // Velocidad de arrastre (simulando el aire)
        speed *= (1 - Time.deltaTime * drag);
        yspeed += gAccel * Time.deltaTime;
        transform.Translate(0, yspeed, speed);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShell : MonoBehaviour
{
    public GameObject bullet;
    public GameObject turet;
    public GameObject enemy;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 aimAt = CalculateTrajectory();

            if (aimAt != Vector3.zero)
                transform.forward = aimAt;
            CreateBullet();
        }
    }

    void CreateBullet()
    {
        Instantiate(bullet, turet.transform.position, turet.transform.rotation);
    }

    Vector3 CalculateTrajectory()
    {
        /*
            bullet => s*t   (s=speed)
            target => p + v*t (p = position, v = velocity)

            p + v*t = s*t

            (p + v*t)^2 = (s*t)^2 

            (p + v*t)^2 - (s*t)^2 = 0

            p*p + 2(p*v)t + (v*v)t^2  - (s^2)t^2 = 0
          
            p*p + 2(p*v)t + (v*v-s^2)t^2 = 0;
            
            ax^2 + bx + C =0
            
            x = -b+-sqrt(b^2 - 4ac)
                ---------------
                    2a
         */

        Vector3 p = enemy.transform.position - transform.position;
        Vector3 v = enemy.transform.forward * enemy.GetComponent<Drive>().speed;
        float s = bullet.GetComponent<MoveShell>().speed;

        float a = Vector3.Dot(v, v) - s * s;
        // Resultado simplificado, cancelado 4 y 2 de la ecuación de segundo grado
        float b = Vector3.Dot(v, p);
        float c = Vector3.Dot(p, p);
        // Resultado simplificado, cancelado 4 y 2 de la ecuación de segundo grado
        float d = b * b - 4 * a * c;

        // Si el valor de d es muy pequeño, la bala no va tener la suficiente velocidad para alcanzar el enemigo
        // Este calculo lo hacemos antes de hacer la raiz quadrada de d
        if (d < 0.1f)
            return Vector3.zero;

        float sqrt = Mathf.Sqrt(d);
        float t1 = (-b - sqrt) / c;
        float t2 = (-b + sqrt) / c;

        float t = 0;

        if (t1 < 0 && t2 < 0)
            //t = 0;
            return Vector3.zero;
        else if (t1 < 0)
            t = t2;
        else if (t2 < 0)
            t = t1;
        else
        {
            t = Mathf.Max(new float[] { t1, t2 });
        }

        return t * p + v;
    }
}

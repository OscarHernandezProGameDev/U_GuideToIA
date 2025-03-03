using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShell : MonoBehaviour
{
    public GameObject bullet;
    public GameObject turret;
    public GameObject enemy;

    float rotSpeed = 2f;

    void CreateBullet()
    {

        Instantiate(bullet, turret.transform.position, turret.transform.rotation);
    }

    void Update()
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateBullet();
        }
    }
}

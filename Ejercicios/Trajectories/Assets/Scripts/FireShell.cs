using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShell : MonoBehaviour
{
    public GameObject bullet;
    public GameObject turret;
    public GameObject enemy;
    public Transform turretBase;

    float speed = 15;
    float rotSpeed = 2f;
    void Update()
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
        RotateTurret();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateBullet();
        }
    }
    void CreateBullet()
    {

        Instantiate(bullet, turret.transform.position, turret.transform.rotation);
    }

    void RotateTurret()
    {
        float? angle = CalculateAngle(true);

        if(angle.HasValue)
            turretBase.localEulerAngles=new Vector3(360f-angle.Value, 0, 0);
    }
    float? CalculateAngle(bool low)
    {
        Vector3 targetDir = enemy.transform.position - transform.position;
        float y = targetDir.y;

        targetDir.y = 0;
        float x = targetDir.magnitude;
        float gravity = 9.8f;
        float sSqrt = speed * speed;
        float underTheSqrRoot = (sSqrt * sSqrt) - gravity * (gravity * x * x + 2 * y * sSqrt);

        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqrt + root;
            float lowAngle = sSqrt - root;

            return low ? Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg : Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg;
        }

        return null;
    }
}

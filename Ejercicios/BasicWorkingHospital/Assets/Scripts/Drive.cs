using UnityEngine;

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 20.0f;

    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        cam.gameObject.transform.LookAt(transform.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float translation2 = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        this.transform.Translate(0, 0, translation);
        this.transform.Translate(translation2, 0, 0);

        if (Input.GetKey(KeyCode.Z))
            this.transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.C))
            this.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.R) && cam.gameObject.transform.position.y > 5)
            cam.gameObject.transform.Translate(0, 0, speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.F) && cam.gameObject.transform.position.y < 45)
            cam.gameObject.transform.Translate(0, 0, -speed * Time.deltaTime);

        float angle = Vector3.Angle(cam.gameObject.transform.forward, Vector3.up);
        //Debug.Log(angle);

        if (Input.GetKey(KeyCode.T) && angle < 175)
        {
            cam.gameObject.transform.Translate(Vector3.up);
            cam.gameObject.transform.LookAt(transform.position);
        }

        if (Input.GetKey(KeyCode.G) && angle > 95)
        {
            cam.gameObject.transform.Translate(Vector3.down);
            cam.gameObject.transform.LookAt(transform.position);
        }
    }
}

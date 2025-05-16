using UnityEngine;

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 20.0f;

    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float translation2 = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        this.transform.Translate(0, 0, translation);
        this.transform.Translate(translation2, 0, 0);
    }
}

using UnityEngine;
using System.Collections;

public class SimpleCamera : MonoBehaviour
{
    
    private Vector3 lastPosition;
    private Vector3 firstPosition;
    private float rotationX = 0f;
    private float rotationY = 0f;

    public float speed = 10f;
    public float mouseSensitivity = 2f;

    public bool IsActive { get; set; }

    void Start()
    {
        IsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsActive) return;
        Mouse();
        Keyboard();
    }

    private void Mouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rotationX = transform.eulerAngles.x;
            rotationY = transform.eulerAngles.y;
        }
 
        if (Input.GetMouseButton(1))
        {

            rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY += Input.GetAxis("Mouse X") * mouseSensitivity; 

            // [0 - 90] [330 - 360]
            int limDown = 90;
            int limUp = 330;
            if ( rotationX > limDown && rotationX < limUp )
                if (Mathf.Abs(rotationX - limDown) < Mathf.Abs(rotationX - limUp))
                    rotationX = limDown;
                else
                    rotationX = limUp;

            transform.eulerAngles = new Vector3(rotationX,
                                                rotationY,
                                                transform.eulerAngles.z);

            //Debug.Log("Camera at " + transform.eulerAngles);
        }

        if(Input.GetMouseButtonUp(1))
        {
            //Debug.Log("Release camera at " + transform.eulerAngles);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * speed);
        }  
    }

    private void Keyboard()
    {
        float s = Time.deltaTime * speed;

        if ( Input.GetKey( KeyCode.W )  )
        {
            transform.Translate(s * Mathf.Sin( transform.eulerAngles.y * Mathf.Deg2Rad), 0, 
                                s * Mathf.Cos( transform.eulerAngles.y * Mathf.Deg2Rad ), Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-s * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad), 0,
                                -s * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad), Space.World);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(+s * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad), 0,
                                -s * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad), Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-s * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad), 0,
                                +s * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad), Space.World);
        }

    }
}

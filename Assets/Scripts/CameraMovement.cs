using System;
using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 15f;
    public float sensitivity = 10f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float maxPitch = 90f;
    public float minPitch = -90f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        yaw += sensitivity * Input.GetAxis("Mouse X") * Time.deltaTime * 60;
        pitch -= sensitivity * Input.GetAxis("Mouse Y") * Time.deltaTime * 60;

        // Clamp the pitch value to prevent flipping
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Apply the rotations
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // Movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * (speed * Time.deltaTime), Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-transform.forward * (speed * Time.deltaTime), Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-transform.right * (speed * Time.deltaTime), Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * (speed * Time.deltaTime), Space.World);
        }
    }
}
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 _offset;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _offset = transform.position - target.position;
    }

    private void Update()
    {
        transform.position = target.position + _offset;
    }
}
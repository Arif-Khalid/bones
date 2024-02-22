using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float _maxAbsoluteXRotation = 60.0f;
    [SerializeField] private float _mouseSens = 100f;

    private float xRotation = 0f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * _mouseSens;
        xRotation -= mouseDelta.y * Time.deltaTime;
        xRotation = Mathf.Max(xRotation, -_maxAbsoluteXRotation);
        xRotation = Mathf.Min(xRotation, _maxAbsoluteXRotation);

        transform.Rotate(mouseDelta.x * Time.deltaTime * Vector3.up);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}

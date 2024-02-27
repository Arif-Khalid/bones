using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Responsible for handling player camera rotation through mouse movement
 */
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float _maxAbsoluteXRotation = 60.0f;
    [SerializeField] private float _mouseSens = 100f;

    private float _xRotation = 0f;
    private bool _isLookEnabled = true;
    private void Start() {
        GameManager.OnStartGame += EnableLook;
        GameManager.OnEndGame += DisableLook;
    }

    private void EnableLook() {
        _isLookEnabled = true;
    }

    private void DisableLook() {
        _isLookEnabled = false;
    }

    private void Update()
    {
        if(!_isLookEnabled) {
            return;
        }
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * _mouseSens;
        _xRotation -= mouseDelta.y * Time.deltaTime;
        _xRotation = Mathf.Max(_xRotation, -_maxAbsoluteXRotation);
        _xRotation = Mathf.Min(_xRotation, _maxAbsoluteXRotation);

        transform.Rotate(mouseDelta.x * Time.deltaTime * Vector3.up);
        Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void OnDestroy() {
        GameManager.OnStartGame -= EnableLook;
        GameManager.OnEndGame -= DisableLook;
    }
}

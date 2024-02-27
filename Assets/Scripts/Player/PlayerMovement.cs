using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Responsible for handling player transform movement through character controller
 */
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;

    // Input reading
    private Vector2 _moveVal2D = Vector2.zero;
    private float _playerYVelocity = 0f;

    // Input processing
    private Vector3 _moveVal3D = Vector3.zero;

    private CharacterController _characterController = null;

    private void Start() {
        _characterController = GetComponent<CharacterController>();
    }

    // Called by player input component
    private void OnMove(InputValue value) {
        _moveVal2D = value.Get<Vector2>();
    }

    private void ProcessInputs() {
        if (!_characterController.isGrounded) {
            _playerYVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else {
            _playerYVelocity = 0f;
        }
        _moveVal3D = transform.right * _moveVal2D.x + transform.forward * _moveVal2D.y;
        _moveVal3D = new Vector3(_moveVal3D.x, _playerYVelocity, _moveVal3D.z);
    }

    // Carry out movement in update since character controller is not physics based
    private void Update() {
        ProcessInputs();
        _characterController.Move(_moveVal3D * _speed * Time.deltaTime);
    }
}

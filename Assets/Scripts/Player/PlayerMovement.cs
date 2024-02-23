using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _jumpPower = 1.0f;
    [SerializeField] private float _simulatedGravity = -2.0f;
    [SerializeField] private float _terminalVelocity = -5.0f;
    [SerializeField] private float _acceleration = 1.0f;

    // Input reading
    private Vector2 _moveVal2D = Vector2.zero;
    private bool _hasJumped = false;

    // Input processing
    private Vector3 _moveVal3D = Vector3.zero;
    private float _playerYVelocity = 0.0f;

    private CharacterController _characterController = null;

    private void Start() {
        _characterController = GetComponent<CharacterController>();
    }

    #region Reading inputs
    private void OnMove(InputValue value) {
        _moveVal2D = value.Get<Vector2>();
    }

    private void OnJump(InputValue value) {
        _hasJumped = true;
    }
    #endregion

    private void ProcessInputs() {
        if(_hasJumped && _characterController.isGrounded) {
            _playerYVelocity = _jumpPower;
            _hasJumped = false;
        }

        if(!_characterController.isGrounded) {
            _playerYVelocity += _simulatedGravity;
        }
        _playerYVelocity = Mathf.Max(_playerYVelocity, _terminalVelocity);
        Vector3 desiredMove = transform.right * _moveVal2D.x + transform.forward * _moveVal2D.y;

        _moveVal3D = Vector3.Lerp(_moveVal3D, new Vector3(desiredMove.x, _playerYVelocity, desiredMove.z), _acceleration);
    }

    // Carry out movement in update since character controller is not physics based
    private void Update() {
        ProcessInputs();
        _characterController.Move(_moveVal3D * _speed * Time.deltaTime);
    }
}

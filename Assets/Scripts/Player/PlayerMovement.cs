using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;

    // Input reading
    private Vector2 _moveVal2D = Vector2.zero;

    // Input processing
    private Vector3 _moveVal3D = Vector3.zero;

    private CharacterController _characterController = null;

    private void Start() {
        _characterController = GetComponent<CharacterController>();
    }

    #region Reading inputs
    private void OnMove(InputValue value) {
        _moveVal2D = value.Get<Vector2>();
    }
    #endregion

    private void ProcessInputs() {
        _moveVal3D = transform.right * _moveVal2D.x + transform.forward * _moveVal2D.y;
    }

    // Carry out movement in update since character controller is not physics based
    private void Update() {
        ProcessInputs();
        _characterController.Move(_moveVal3D * _speed * Time.deltaTime);
    }
}

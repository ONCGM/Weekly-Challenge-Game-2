using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Player Settings")]
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private bool canMove = true;

    const float movementTolerance = 0.05f;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;

    public bool isMoving { get; private set; }

    // Components
    private CharacterController characterController;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate() {
        if (!canMove) return;
        Movement();
    }

    private void Movement() {
        var xAxis = Input.GetAxisRaw("Horizontal");
        var zAxis = Input.GetAxisRaw("Vertical");

        var movement = (new Vector3(xAxis, 0f, zAxis)).normalized;
        

        if (movement.magnitude >= movementTolerance) {
            var targetAngle = Mathf.Atan2(movement.x, movement.z) *
            Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            var moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            characterController.Move(moveDir * playerSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            isMoving = true;
        } else isMoving = false;
    }

}

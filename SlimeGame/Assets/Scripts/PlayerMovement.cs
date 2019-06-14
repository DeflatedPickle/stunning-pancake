using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public LayerMask GroundLayerMask;
    public float GroundSphere = 0.3f;
    public bool IsOnGround;
    
    public float JumpForce = 4;
    public float JumpForceHeld = 8;
    public float JumpTimeTotal = 20f;
    public float JumpTimeCurrent;

    private InputMaster _inputMaster;
    private Rigidbody _rigidbody;

    private bool _pressedJump;

    private void Awake() {
        _inputMaster = new InputMaster();

        _rigidbody = GetComponent<Rigidbody>();

        _inputMaster.Player.Movement.performed += ctx => Move(ctx);
        _inputMaster.Player.Jump.performed += ctx => Jump(ctx);
    }

    private void FixedUpdate() {
        IsOnGround = Physics.CheckSphere(transform.position, GroundSphere, GroundLayerMask);
        
        if (_pressedJump) {
            if (IsOnGround) {
                _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                JumpTimeCurrent = JumpTimeTotal;
            }

            if (_rigidbody.velocity.y > 1 && JumpTimeCurrent > 0) {
                _rigidbody.AddForce(Vector3.up * JumpForceHeld, ForceMode.Impulse);
                JumpTimeCurrent -= 1;
            }
        }
    }

    private void Jump(InputAction.CallbackContext ctx) {
        Debug.Log("Jumped, " + ctx.ReadValueAsObject());

        // ReSharper disable once SwitchStatementMissingSomeCases
        switch ((int) Math.Round((float) ctx.ReadValueAsObject())) {
            case 0:
                _pressedJump = false;
                break;
            case 1:
                _pressedJump = true;
                break;
        }
    }

    private void Move(InputAction.CallbackContext ctx) {
        var direction = ctx.ReadValue<Vector2>();
        Debug.Log("Moved by: " + direction);
    }

    private void OnEnable() {
        _inputMaster.Enable();
    }

    private void OnDisable() {
        _inputMaster.Disable();
    }
}
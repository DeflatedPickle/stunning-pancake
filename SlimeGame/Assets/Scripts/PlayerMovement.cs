using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    public LayerMask GroundLayerMask;
    public Vector3 GroundSphereOffset = new Vector3(0, 0.5f, 0);
    public float GroundSphere = 0.4f;
    public bool IsOnGround;
    
    public float JumpForce = 4;
    public float JumpForceHeld = 8;
    public Timer JumpTimer = new Timer();

    private InputMaster _inputMaster;
    private Rigidbody _rigidbody;

    private bool _pressedJump;
    private Vector2 _movementDirection;

    private void Awake() {
        _inputMaster = new InputMaster();

        _rigidbody = GetComponent<Rigidbody>();

        _inputMaster.Player.Movement.performed += ctx => Move(ctx);
        _inputMaster.Player.Jump.performed += ctx => Jump(ctx);
    }

    private void FixedUpdate() {
        IsOnGround = Physics.CheckSphere(transform.position - GroundSphereOffset, GroundSphere, GroundLayerMask);
        
        if (_pressedJump) {
            if (IsOnGround) {
                _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                JumpTimer.Reset();
            }

            if (_rigidbody.velocity.y > 1 && JumpTimer.Current > 0) {
                _rigidbody.AddForce(Vector3.up * JumpForceHeld, ForceMode.Impulse);
                JumpTimer.Tick();
            }
        }

        if (IsOnGround) {
            _rigidbody.AddForce(new Vector3());
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

        _movementDirection = direction;
    }

    private void OnEnable() {
        _inputMaster.Enable();
    }

    private void OnDisable() {
        _inputMaster.Disable();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - GroundSphereOffset, GroundSphere);
    }
}
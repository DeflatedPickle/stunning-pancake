using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JarMovement : MonoBehaviour {
    public Transform Target;
    
    [Serializable]
    public class GroundCategory {
        public LayerMask LayerMask;
        public Vector3 SphereOffset = new Vector3(0, 0.5f, 0);
        public float Sphere = 0.4f;
        public bool IsOnGround;
    }

    public GroundCategory Ground;

    [Serializable]
    public class JumpCategory {
        public float Force = 1;
        public float ForceHeld = 0.2f;
        public Timer Timer = new Timer();

        [ReadOnly] public bool IsJumping;
    }

    public JumpCategory Jump;

    private InputMaster _inputMaster;
    private Rigidbody _rigidbody;

    [Serializable]
    public class MovementCategory {
        public float Force = 4;

        [ReadOnly] public float Direction;
        [ReadOnly] public bool IsMoving;
    }

    public MovementCategory Movement;

    [Serializable]
    public class RotationCategory {
        public float Amount = 4;

        [ReadOnly] public float Direction;
        [ReadOnly] public bool IsRotating;
    }

    public RotationCategory Rotation;

    private void Awake() {
        _inputMaster = new InputMaster();

        _rigidbody = GetComponent<Rigidbody>();

        _inputMaster.Player.Movement.performed += ctx => DoMove(ctx);
        _inputMaster.Player.Jump.performed += ctx => DoJump(ctx);
        _inputMaster.Player.Rotation.performed += ctx => DoRotate(ctx);
    }

    private void FixedUpdate() {
        Ground.IsOnGround =
            Physics.CheckSphere(transform.position - Ground.SphereOffset, Ground.Sphere, Ground.LayerMask);

        if (Jump.IsJumping) {
            if (Ground.IsOnGround) {
                _rigidbody.AddForce(Vector3.up * Jump.Force, ForceMode.Impulse);
                Jump.Timer.Reset();
            }

            if (_rigidbody.velocity.y > 1 && Jump.Timer.Current > 0) {
                _rigidbody.AddForce(Vector3.up * Jump.ForceHeld, ForceMode.Impulse);
                Jump.Timer.Tick();
            }
        }

        if (Movement.IsMoving) {
            if (Ground.IsOnGround) {
                _rigidbody.AddForce(Target.transform.forward * Movement.Force * Movement.Direction);
            }
        }

        if (Rotation.IsRotating) {
            Vector2 rotationDirection = new Vector2();

            if (Rotation.Direction > 0) {
                rotationDirection = -Target.transform.right;
            }
            else if (Rotation.Direction < 0) {
                rotationDirection = Target.transform.right;
            }
            
            transform.Rotate(rotationDirection, Rotation.Amount);
        }
    }

    private void DoJump(InputAction.CallbackContext ctx) {
        Debug.Log("Jumped, " + ctx.ReadValueAsObject());

        // ReSharper disable once SwitchStatementMissingSomeCases
        switch ((int) Math.Round((float) ctx.ReadValueAsObject())) {
            case 0:
                Jump.IsJumping = false;
                break;
            case 1:
                Jump.IsJumping = true;
                break;
        }
    }

    private void DoMove(InputAction.CallbackContext ctx) {
        var direction = ctx.ReadValue<float>();
        Movement.Direction = direction;
        Debug.Log("Moved by: " + direction);

        // ReSharper disable once SwitchStatementMissingSomeCases
        if (Math.Abs(Movement.Direction) > 0) {
            Movement.IsMoving = true;
        }
        else {
            Movement.IsMoving = false;
        }
    }

    private void DoRotate(InputAction.CallbackContext ctx) {
        Debug.Log("Rotated by: " + ctx);

        switch ((int) Math.Round((float) ctx.ReadValueAsObject())) {
            case -1:
                Rotation.IsRotating = true;
                break;
            case 1:
                Rotation.IsRotating = true;
                break;
            default:
                Rotation.IsRotating = false;
                break;
        }

        Rotation.Direction = (float) ctx.ReadValueAsObject();
    }

    private void OnEnable() {
        _inputMaster.Enable();
    }

    private void OnDisable() {
        _inputMaster.Disable();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - Ground.SphereOffset, Ground.Sphere);
    }
}
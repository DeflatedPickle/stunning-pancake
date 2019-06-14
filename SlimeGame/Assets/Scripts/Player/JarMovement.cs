using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JarMovement : GenericMovement {
    public Transform Target;
    
    private void Awake() {
        InputMaster = new InputMaster();

        Rigidbody = GetComponent<Rigidbody>();

        InputMaster.Player.Movement.performed += ctx => DoMove(ctx);
        InputMaster.Player.Jump.performed += ctx => DoJump(ctx);
        // InputMaster.Player.Rotation.performed += ctx => DoRotate(ctx);
    }

    private void FixedUpdate() {
        Ground.IsOnGround =
            Physics.CheckSphere(transform.position - Ground.SphereOffset, Ground.Sphere, Ground.LayerMask);

        if (Jump.IsJumping) {
            if (Ground.IsOnGround) {
                Rigidbody.AddForce(Vector3.up * Jump.Force, ForceMode.Impulse);
                Jump.Timer.Reset();
            }

            if (Rigidbody.velocity.y > 1 && Jump.Timer.Current > 0) {
                Rigidbody.AddForce(Vector3.up * Jump.ForceHeld, ForceMode.Impulse);
                Jump.Timer.Tick();
            }
        }

        if (Movement.IsMoving) {
            if (Ground.IsOnGround) {
                Rigidbody.AddForce(Target.transform.forward * Movement.Force * Movement.Direction);
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
}
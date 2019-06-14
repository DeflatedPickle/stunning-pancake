using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeMovement : GenericMovement {
    private void Awake() {
        InputMaster = new InputMaster();

        Rigidbody = GetComponent<Rigidbody>();

        InputMaster.Player.Movement.performed += ctx => DoMove(ctx);
        InputMaster.Player.Jump.performed += ctx => DoJump(ctx);
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
                Rigidbody.AddForce(new Vector3(Movement.Force * Movement.VectorDirection.x, 0,
                    Movement.Force * Movement.VectorDirection.y));
            }
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
        var direction = ctx.ReadValue<Vector2>();
        Movement.VectorDirection = direction;
        Debug.Log("Moved by: " + direction);

        if (Math.Abs(Movement.VectorDirection.x) > 0 || Math.Abs(Movement.VectorDirection.y) > 0) {
            Movement.IsMoving = true;
        }
        else {
            Movement.IsMoving = false;
        }
    }
}
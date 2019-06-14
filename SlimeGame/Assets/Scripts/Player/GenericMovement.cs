using System;
using UnityEngine;

public class GenericMovement : MonoBehaviour {
    protected InputMaster InputMaster;
    protected Rigidbody Rigidbody;
    
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

    [Serializable]
    public class MovementCategory {
        public float Force = 4;

        [ReadOnly] public Vector2 VectorDirection;
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

    private void OnEnable() {
        InputMaster.Enable();
    }

    private void OnDisable() {
        InputMaster.Disable();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - Ground.SphereOffset, Ground.Sphere);
    }
}
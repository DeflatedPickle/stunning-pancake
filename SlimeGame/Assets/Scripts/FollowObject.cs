using UnityEngine;

public class FollowObject : MonoBehaviour {
    public bool copyPositionX;
    public bool copyPositionY;
    public bool copyPositionZ;

    public bool copyRotationX;
    public bool copyRotationY;
    public bool copyRotationZ;
    public bool copyRotationW;

    public GameObject parent;

    void Update() {
        var position = transform.position;
        if (copyPositionX) {
            position.x = parent.transform.position.x;
        }

        if (copyPositionY) {
            position.y = parent.transform.position.y;
        }

        if (copyPositionZ) {
            position.z = parent.transform.position.z;
        }

        transform.position = position;

        if (copyRotationX) {
            transform.rotation = new Quaternion(parent.transform.rotation.x, 0, 0, 0);
        }

        if (copyRotationY) {
            transform.rotation = new Quaternion(0, parent.transform.rotation.y, 0, 0);
        }

        if (copyRotationZ) {
            transform.rotation = new Quaternion(0, 0, parent.transform.rotation.z, 0);
        }

        if (copyRotationW) {
            transform.rotation = new Quaternion(0, 0, 0, parent.transform.rotation.w);
        }
    }
}
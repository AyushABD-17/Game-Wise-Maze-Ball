// using UnityEngine;

// public class CameraFollow : MonoBehaviour
// {
//     public Transform target;
//     public Vector3 offset = new Vector3(0, 8, -6);
//     public float smoothTime = 0.15f;
//     Vector3 velocity = Vector3.zero;

//     void LateUpdate()
//     {
//         if (target == null) return;
//         Vector3 desired = target.position + offset;
//         transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
//         transform.LookAt(target.position + Vector3.up * 0.5f);
//     }
// }


using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Vector3 fixedPosition = new Vector3(0, 15, -15);
    public Vector3 fixedRotation = new Vector3(45, 0, 0);

    void Start()
    {
        transform.position = fixedPosition;
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}

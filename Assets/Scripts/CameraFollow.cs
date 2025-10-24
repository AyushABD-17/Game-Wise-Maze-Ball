using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 8, -6);
    public float smoothTime = 0.15f;
    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        transform.LookAt(target.position + Vector3.up * 0.5f);
    }
}

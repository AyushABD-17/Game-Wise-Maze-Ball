using UnityEngine;

public class GyroCameraFollow : MonoBehaviour
{
    public Transform target;        // Ball to follow
    public Vector3 offset = new Vector3(0, 6, -7); // Camera position relative to ball
    public float followSmooth = 0.1f;  // How smoothly the camera follows position
    public float rotationSmooth = 5f;  // How smoothly it rotates with gyro

    private Vector3 followVelocity = Vector3.zero;
    private bool gyroEnabled;
    private Gyroscope gyro;

    void Start()
    {
        // Enable gyro if supported
        gyroEnabled = EnableGyro();
    }

    bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }
        return false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Smoothly follow the ball position
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref followVelocity, followSmooth);

        // If gyro available, rotate camera according to device tilt
        if (gyroEnabled)
        {
            // Convert gyro attitude to Unity rotation (mobile-friendly)
            Quaternion deviceRotation = new Quaternion(gyro.attitude.x, gyro.attitude.y, -gyro.attitude.z, -gyro.attitude.w);
            Quaternion targetRotation = Quaternion.Euler(90, 0, 0) * deviceRotation;

            // Smooth rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmooth * Time.deltaTime);
        }
        else
        {
            // Mouse fallback (Editor testing)
            float mouseX = Input.GetAxis("Mouse X") * 3f;
            float mouseY = -Input.GetAxis("Mouse Y") * 3f;
            transform.RotateAround(target.position, Vector3.up, mouseX);
            transform.RotateAround(target.position, transform.right, mouseY);
        }
    }
}

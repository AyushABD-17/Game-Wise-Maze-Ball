using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public float gyroStrength = 12f;        // force multiplier when using gyro/accel
    public float keyboardStrength = 8f;     // editor fallback
    public float maxSpeed = 6f;
    public bool useGyro = false;             // toggle

    Rigidbody rb;

    // low-pass filter for accelerometer
    Vector3 accelFiltered = Vector3.zero;
    public float accelFilterFactor = 0.2f; // 0..1 (lower = smoother)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Input.gyro.enabled = true;
        accelFiltered = Input.acceleration;
    }

    void FixedUpdate()
    {
        Vector3 force = Vector3.zero;

        if (useGyro && SystemInfo.supportsGyroscope)
        {
            // use acceleration (works in editor as well via device simulators)
            Vector3 acc = Input.acceleration;
            // smooth
            accelFiltered = Vector3.Lerp(accelFiltered, acc, accelFilterFactor);
            // depending on device orientation you may need to swap axes
            Vector3 dir = new Vector3(accelFiltered.x, 0f, accelFiltered.y);
            // convert to world space relative to camera
            dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0f;
            force = dir * gyroStrength;
        }
        else
        {
            // fallback controls for editor (WASD / arrow)
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = new Vector3(h, 0f, v);
            // align with camera forward
            dir = Camera.main.transform.TransformDirection(dir);
            dir.y = 0f;
            force = dir * keyboardStrength;
        }

        // apply force but don't exceed max speed
        if (rb.linearVelocity.magnitude < maxSpeed)
            rb.AddForce(force, ForceMode.Force);

        // optional: slow down if no input
        // rb.velocity *= 0.999f;
    }
}

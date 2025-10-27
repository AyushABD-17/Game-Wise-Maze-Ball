// // 

// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// public class BallController : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     public float gyroStrength = 12f;       // force multiplier for gyro input
//     public float keyboardStrength = 8f;    // fallback for editor
//     public float maxSpeed = 6f;            // maximum ball speed
//     public bool useGyro = true;            // enable gyro control

//     [Header("Smoothing Settings")]
//     public float accelFilterFactor = 0.5f; // 0..1, higher = more responsive

//     Rigidbody rb;
//     Vector3 accelFiltered = Vector3.zero;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.interpolation = RigidbodyInterpolation.Interpolate; // smoother physics movement
//         rb.linearDamping = 0.5f;       // slows naturally when no input
//         rb.angularDamping = 0.05f;

//         if (useGyro && SystemInfo.supportsGyroscope)
//         {
//             Input.gyro.enabled = true;
//             accelFiltered = Input.acceleration ;
//         }
//     }

//     void FixedUpdate()
//     {
//         Vector3 force = Vector3.zero;

//         if (useGyro && SystemInfo.supportsGyroscope)
//         {
//             // get gyro / accelerometer input
//             Vector3 acc = Input.acceleration;

//             // smooth input for stability
//             accelFiltered = Vector3.Lerp(accelFiltered, acc, accelFilterFactor);

//             // map movement relative to camera orientation
//             Vector3 camForward = Camera.main.transform.forward;
//             Vector3 camRight = Camera.main.transform.right;
//             camForward.y = 0; camRight.y = 0;
//             camForward.Normalize(); camRight.Normalize();

//             Vector3 dir = camRight * accelFiltered.x + camForward * accelFiltered.y;

//             // scale force based on tilt magnitude for responsiveness
//             float tiltMag = new Vector2(accelFiltered.x, accelFiltered.y).magnitude;
//             force = dir * gyroStrength * Mathf.Clamp01(tiltMag * 3f);
//         }
//         else
//         {
//             // fallback: keyboard / editor input
//             float h = Input.GetAxis("Horizontal");
//             float v = Input.GetAxis("Vertical");

//             Vector3 camForward = Camera.main.transform.forward;
//             Vector3 camRight = Camera.main.transform.right;
//             camForward.y = 0; camRight.y = 0;
//             camForward.Normalize(); camRight.Normalize();

//             Vector3 dir = camRight * h + camForward * v;
//             force = dir * keyboardStrength;
//         }

//         // apply movement
//         rb.AddForce(force, ForceMode.Force);

//         // clamp maximum speed
//         if (rb.linearVelocity.magnitude > maxSpeed)
//             rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
//     }
// }



// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// public class BallController : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     public float gyroStrength = 12f;
//     public float keyboardStrength = 8f;
//     public float maxSpeed = 6f;
//     public bool useGyro = true;

//     [Header("Smoothing Settings")]
//     public float accelFilterFactor = 0.5f;

//     Rigidbody rb;
//     Vector3 accelFiltered = Vector3.zero;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.interpolation = RigidbodyInterpolation.Interpolate;
//         rb.linearDamping = 0.5f;
//         rb.angularDamping = 0.05f;

//         if (useGyro && SystemInfo.supportsGyroscope)
//         {
//             Input.gyro.enabled = true;
//             accelFiltered = Input.acceleration;
//         }
//     }

//     void FixedUpdate()
//     {
//         Vector3 force = Vector3.zero;

//         if (useGyro && SystemInfo.supportsGyroscope)
//         {
//             // Get tilt input
//             Vector3 acc = Input.acceleration;
//             accelFiltered = Vector3.Lerp(accelFiltered, acc, accelFilterFactor);

//             // Use camera orientation to determine direction (even if static)
//             Transform cam = Camera.main.transform;
//             Vector3 camForward = cam.forward;
//             Vector3 camRight = cam.right;
//             camForward.y = 0; camRight.y = 0;
//             camForward.Normalize(); camRight.Normalize();

//             // Gyro tilt controls (X = left-right, Y = forward-back)
//             Vector3 dir = camRight * accelFiltered.x + camForward * accelFiltered.y;

//             float tiltMag = new Vector2(accelFiltered.x, accelFiltered.y).magnitude;
//             force = dir * gyroStrength * Mathf.Clamp01(tiltMag * 3f);
//         }
//         else
//         {
//             // Keyboard fallback
//             float h = Input.GetAxis("Horizontal");
//             float v = Input.GetAxis("Vertical");

//             Transform cam = Camera.main.transform;
//             Vector3 camForward = cam.forward;
//             Vector3 camRight = cam.right;
//             camForward.y = 0; camRight.y = 0;
//             camForward.Normalize(); camRight.Normalize();

//             Vector3 dir = camRight * h + camForward * v;
//             force = dir * keyboardStrength;
//         }

//         // Apply movement
//         rb.AddForce(force, ForceMode.Force);

//         // Clamp speed
//         if (rb.linearVelocity.magnitude > maxSpeed)
//             rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
//     }
// }


using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float gyroStrength = 12f;
    public float keyboardStrength = 8f;
    public float maxSpeed = 6f;
    public bool useGyro = true;

    [Header("Smoothing Settings")]
    public float accelFilterFactor = 0.5f;

    private Rigidbody rb;
    private Vector3 accelFiltered = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.05f;

        if (useGyro && SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            accelFiltered = Input.acceleration;
        }
    }

    void FixedUpdate()
    {
        Vector3 force = Vector3.zero;

        if (useGyro && SystemInfo.supportsGyroscope)
        {
            Vector3 acc = Input.acceleration;
            accelFiltered = Vector3.Lerp(accelFiltered, acc, accelFilterFactor);

            // Ignore camera — use world axes directly
            Vector3 dir = new Vector3(accelFiltered.x, 0, accelFiltered.y);

            float tiltMag = new Vector2(accelFiltered.x, accelFiltered.y).magnitude;
            force = dir * gyroStrength * Mathf.Clamp01(tiltMag * 3f);
        }
        else
        {
            // fallback for keyboard
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dir = new Vector3(h, 0, v);
            force = dir * keyboardStrength;
        }

        rb.AddForce(force, ForceMode.Force);

        // Clamp maximum speed
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}

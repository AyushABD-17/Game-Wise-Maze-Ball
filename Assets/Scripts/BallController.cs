using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float gyroStrength = 100f;
    public float keyboardStrength = 8f;
    public float maxSpeed = 6f;
    public bool useGyro = true;

    [Header("Smoothing Settings")]
    public float accelFilterFactor = 0.8f;

    private Rigidbody rb;
    private Vector3 accelFiltered = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = 0.0f;
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
            force = dir * gyroStrength * Mathf.Clamp01(tiltMag * 30f);
        }
        else
        {
            // fallback for keyboard
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dir = new Vector3(h, 0, v);
            force = dir * keyboardStrength;
        }

        rb.AddForce(force, ForceMode.Acceleration);

        // Clamp maximum speed
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }
}




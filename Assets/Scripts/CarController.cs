using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Attributes")]
    public float topSpeed = 50f;
    private float maxAcceleration;
    public float downforce = 100f;
    public float fuel = 100f;
    public float tyreGrip = 1f;
    public float aeroEfficiency = 0.9f;
    public float pitStopTime = 5f;

    [Header("Car Physics")]
    public float brakingForce = 3000f;
    public float accelerationRate = 5f;
    public float brakingRate = 7f;
    public float decelerationRate = 2f;

    private Rigidbody rb;
    private float currentThrottle = 0f;
    private float currentSteering = 0f;
    private bool isOutOfFuel = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing from the car.");
        }

        maxAcceleration = (topSpeed * topSpeed) / 100f;
    }

    private void FixedUpdate()
    {
        if (isOutOfFuel) return;

        ApplyMovement();
        ApplySteering();
        ApplyDownforce();
        ApplyDrag();
        LimitSpeed();
        ConsumeFuel();
    }

    private void ApplyMovement()
    {
        Vector3 force = transform.forward * currentThrottle * maxAcceleration;
        rb.AddForce(force, ForceMode.Force);
    }

    private void ApplySteering()
    {
        rb.AddTorque(Vector3.up * currentSteering * tyreGrip * 50f, ForceMode.Force);
    }

    private void ApplyDownforce()
    {
        rb.AddForce(Vector3.down * downforce, ForceMode.Force);
    }

    private void ApplyDrag()
    {
        float dragFactor = 1f - aeroEfficiency;
        rb.drag = Mathf.Clamp(rb.velocity.magnitude * dragFactor * 0.01f, 0.1f, 2f);
    }

    private void LimitSpeed()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, topSpeed);
    }

    private void ConsumeFuel()
    {
        float speedFactor = rb.velocity.magnitude / topSpeed;
        float accelerationFactor = Mathf.Abs(currentThrottle);
        float timeFactor = Time.fixedDeltaTime;

        fuel -= (speedFactor * 0.05f + accelerationFactor * 0.1f) * timeFactor;

        if (fuel <= 0)
        {
            RunOutOfFuel();
        }
    }

    private void RunOutOfFuel()
    {
        isOutOfFuel = true;
        rb.velocity = Vector3.zero;
        Invoke(nameof(Refuel), pitStopTime);
    }

    private void Refuel()
    {
        isOutOfFuel = false;
        fuel = 100f;
    }

    // ✅ Updated to smoothly apply AI inputs
    public void SetInputs(float throttle, float steering)
    {
        currentThrottle = Mathf.Lerp(currentThrottle, Mathf.Clamp(throttle, -1f, 1f), Time.fixedDeltaTime * 3f);
        currentSteering = Mathf.Lerp(currentSteering, Mathf.Clamp(steering, -1f, 1f), Time.fixedDeltaTime * 3f);
    }
    public void SetAttributes(CarData data)
    {
        this.topSpeed = data.topSpeed;
        this.maxAcceleration = data.acceleration;
        this.downforce = data.downforce;
        this.fuel = data.fuel;
        this.tyreGrip = data.tyreGrip;
        this.aeroEfficiency = data.aeroEfficiency;
        this.pitStopTime = data.pitStopTime;
    }
    public float GetDownforce()
    {
        return downforce;
    }

    //Get Current Speed in km/h
    public float GetCurrentSpeed()
    {
        return rb.velocity.magnitude * 3.6f; // Convert m/s to km/h
    }

    //Get Current Acceleration (change in speed per second)
    public float GetCurrentAcceleration()
    {
        return Mathf.Abs(currentThrottle) * maxAcceleration;
    }

}

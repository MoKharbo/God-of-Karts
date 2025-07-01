using Unity.VisualScripting;
using UnityEngine;

public class Carcontroller : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private WheelColliders colliders;
    [SerializeField] private WheelMeshes meshes;
    [SerializeField] private Enemymovement Enemymovement;
    public float gasInput;
    public float steeringInput;
    [SerializeField] private float motorPower;
    [SerializeField] private float brakePower;
    [SerializeField] private float brakeInput;
    [SerializeField] private float boostAmount = 20f;
    [SerializeField] private float boostDuration = 4f;
    private bool isBoosting = false;
    private float boostEndTime = 0f;
    public KeyCode boostKey = KeyCode.R;
    [SerializeField] Scoredrift score;
    [SerializeField] private float maxSteerAngle = 35f;
    private float baseMaxSpeed = 50f;
    public float maxSpeed = 50f;

    public float speed;

    public KeyCode driftKey = KeyCode.Space;
    private float driftAmount = 0f;
    [SerializeField] private float driftLerpSpeed = 5f;

    public bool isDrifting => Input.GetKey(driftKey);
    private float slipAngle;

    // Properties for the camera script
    public bool IsGrounded { get; private set; }
    public Vector3 GroundNormal { get; private set; } = Vector3.up;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        speed = rb.linearVelocity.magnitude;
        ApplyWheelPositions();

        // Handle boost input
        if (Input.GetKeyDown(boostKey) && !isBoosting && score.score >= 1000)
        {
            score.score -= 1000; // Deduct score for boost
            StartBoost();
        }

        // End boost if time expired
        if (isBoosting && Time.time >= boostEndTime)
        {
            EndBoost();
        }

        Debug.Log($"Motor Torque: {gasInput * motorPower}, Grounded: {colliders.RearLeftWheel.isGrounded && colliders.RearRightWheel.isGrounded}");

        SetArcadeFriction(colliders.FrontLeftWheel);
        SetArcadeFriction(colliders.FrontRightWheel);
        SetArcadeFriction2(colliders.RearLeftWheel);
        SetArcadeFriction2(colliders.RearRightWheel);

        float target = isDrifting ? 1f : 0f;
        driftAmount = Mathf.MoveTowards(driftAmount, target, Time.deltaTime * driftLerpSpeed);
    }

    void FixedUpdate()
    {
        CheckInput();

        // Update grounded state and ground normal based on wheel contacts
        IsGrounded = colliders.RearLeftWheel.isGrounded && colliders.RearRightWheel.isGrounded &&
                     colliders.FrontLeftWheel.isGrounded && colliders.FrontRightWheel.isGrounded;

        if (IsGrounded)
        {
            Vector3 normalSum = Vector3.zero;

            WheelHit hit;
            if (colliders.RearLeftWheel.GetGroundHit(out hit)) normalSum += hit.normal;
            if (colliders.RearRightWheel.GetGroundHit(out hit)) normalSum += hit.normal;
            if (colliders.FrontLeftWheel.GetGroundHit(out hit)) normalSum += hit.normal;
            if (colliders.FrontRightWheel.GetGroundHit(out hit)) normalSum += hit.normal;

            GroundNormal = normalSum.normalized;
        }
        else
        {
            GroundNormal = Vector3.up;
        }

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        ApplySteering();
        ApplyMotor();
        ApplyBrake();
        if (gasInput > 0f && !isDrifting)
        {
            rb.AddForce(transform.forward * 5000f);
        }
        float baseMax = isDrifting ? 70f : 50f;
        float effectiveMaxSpeed = baseMax + (isBoosting ? boostAmount : 0f);
        maxSpeed = effectiveMaxSpeed;
        float driftSpeed = isDrifting ? 70f : maxSpeed;

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Apply force accordingly
        if (gasInput > 0f)
        {
            float force = isDrifting ? 10000f : 5000f;
            rb.AddForce(transform.forward * force);
        }
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");

        slipAngle = Vector3.Angle(transform.forward, rb.linearVelocity - transform.forward);

        if (slipAngle < 120f)
        {
            if (gasInput < 0.5)
            {
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
            else
            {
                brakeInput = 0;
            }
        }
        else
        {
            brakeInput = 0;
        }
    }
    void StartBoost()
    {
        isBoosting = true;
        boostEndTime = Time.time + boostDuration;
        maxSpeed += boostAmount;
        Debug.Log("BOOST STARTED");
    }

    void EndBoost()
    {
        isBoosting = false;
        maxSpeed -= boostAmount;
        Debug.Log("BOOST ENDED");

    }

    void SetArcadeFriction(WheelCollider wheel)
    {
        var forward = wheel.forwardFriction;
        var sideways = wheel.sidewaysFriction;

        forward.stiffness = Mathf.Lerp(3f, 4.5f, driftAmount);
        forward.extremumSlip = Mathf.Lerp(0.4f, 0.8f, driftAmount);
        forward.extremumValue = Mathf.Lerp(1f, 0.85f, driftAmount);
        forward.asymptoteSlip = Mathf.Lerp(0.8f, 1.6f, driftAmount);
        forward.asymptoteValue = Mathf.Lerp(0.5f, 0.25f, driftAmount);

        // SIDEWAYS FRICTION
        sideways.stiffness = Mathf.Lerp(3f, 4.5f, driftAmount);
        sideways.extremumSlip = Mathf.Lerp(0.4f, 0.8f, driftAmount);
        sideways.extremumValue = Mathf.Lerp(1f, 0.85f, driftAmount);
        sideways.asymptoteSlip = Mathf.Lerp(0.8f, 1.6f, driftAmount);
        sideways.asymptoteValue = Mathf.Lerp(0.5f, 0.25f, driftAmount);

        wheel.forwardFriction = forward;
        wheel.sidewaysFriction = sideways;
    }


    void SetArcadeFriction2(WheelCollider wheel)
    {
        var forward = wheel.forwardFriction;
        var sideways = wheel.sidewaysFriction;

        forward.stiffness = Mathf.Lerp(1f, 1f, driftAmount);
        forward.extremumSlip = Mathf.Lerp(0.4f, 1f, driftAmount);
        forward.extremumValue = Mathf.Lerp(1f, 0.8f, driftAmount);
        forward.asymptoteSlip = Mathf.Lerp(0.8f, 1.8f, driftAmount);
        forward.asymptoteValue = Mathf.Lerp(0.5f, 0.2f, driftAmount);

        // SIDEWAYS FRICTION
        sideways.stiffness = Mathf.Lerp(10f, 9f, driftAmount);
        sideways.extremumSlip = Mathf.Lerp(0.4f, 1.1f, driftAmount);
        sideways.extremumValue = Mathf.Lerp(1f, 0.8f, driftAmount);
        sideways.asymptoteSlip = Mathf.Lerp(1f, 2.2f, driftAmount);
        sideways.asymptoteValue = Mathf.Lerp(0.5f, 0.2f, driftAmount);

        wheel.forwardFriction = forward;
        wheel.sidewaysFriction = sideways;
    }


    void ApplyBrake()
    {
        colliders.FrontRightWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.FrontLeftWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.RearRightWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.RearLeftWheel.brakeTorque = brakeInput * brakePower * 0.7f;
    }

    void ApplyMotor()
    {
        colliders.RearRightWheel.motorTorque = gasInput * motorPower;
        colliders.RearLeftWheel.motorTorque = gasInput * motorPower;
    }

    void ApplySteering()
    {
        float speedFactor = Mathf.Clamp01(10f / (rb.linearVelocity.magnitude + 0.1f));
        float steeringAngle = steeringInput * maxSteerAngle * speedFactor;
        colliders.FrontLeftWheel.steerAngle = steeringAngle;
        colliders.FrontRightWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelPositions()
    {
        UpdateWheel(colliders.FrontLeftWheel, meshes.FrontLeftWheel);
        UpdateWheel(colliders.FrontRightWheel, meshes.FrontRightWheel);
        UpdateWheel(colliders.RearLeftWheel, meshes.RearLeftWheel);
        UpdateWheel(colliders.RearRightWheel, meshes.RearRightWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Vector3 position;
        Quaternion rotation;
        coll.GetWorldPose(out position, out rotation);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = rotation;
    }
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider RearLeftWheel;
    public WheelCollider RearRightWheel;
}

[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FrontLeftWheel;
    public MeshRenderer FrontRightWheel;
    public MeshRenderer RearLeftWheel;
    public MeshRenderer RearRightWheel;
}
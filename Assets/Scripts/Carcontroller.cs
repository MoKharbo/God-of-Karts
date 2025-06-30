using UnityEngine;

public class Carcontroller : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private WheelColliders colliders;
    [SerializeField] private WheelMeshes meshes;

    [SerializeField] private float gasInput;
    public float steeringInput;
    [SerializeField] private float motorPower;
    [SerializeField] private float brakePower;
    [SerializeField] private float brakeInput;

    [SerializeField] private float maxSteerAngle = 35f;
    [SerializeField] private float maxSpeed = 20f;

    [SerializeField] private float speed;
    private float slipAngle;

    // Properties for the camera script
    public bool IsGrounded { get; private set; }
    public Vector3 GroundNormal { get; private set; } = Vector3.up;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetArcadeFriction(colliders.FrontLeftWheel);
        SetArcadeFriction(colliders.FrontRightWheel);
    }

    void Update()
    {
        speed = rb.linearVelocity.magnitude;
        ApplyWheelPositions();
        Debug.Log($"Motor Torque: {gasInput * motorPower}, Grounded: {colliders.RearLeftWheel.isGrounded && colliders.RearRightWheel.isGrounded}");
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
        if (gasInput > 0f)
        {
            rb.AddForce(transform.forward * 5000f);
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

    void SetArcadeFriction(WheelCollider wheel)
    {
        var forward = wheel.forwardFriction;
        // You can tweak stiffness here if you want arcade-like friction
        // forward.stiffness = 2f;
        wheel.forwardFriction = forward;

        var sideways = wheel.sidewaysFriction;
        // sideways.stiffness = 2f;
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
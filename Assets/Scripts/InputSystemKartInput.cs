using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemKartInput : KartInput
{
    public PlayerInput PlayerInput;

    public InputActionReference ThrottleAction;
    public InputActionReference BrakeAction;
    public InputActionReference SteeringAction;
    public InputActionReference DriftAction;

    public Renderer carRenderer; // Assign your car's mesh renderer in the Inspector
    public Color driftColor = Color.red;
    public float colorLerpSpeed = 2f;

    private float _throttle;
    private float _brake;
    private float _steering;
    private bool _drift;

    public ParticleSystem driftParticles;
    public ParticleSystem driftParticles2;

    private Color _originalColor;
    private Material _carMaterial;

    private void Awake()
    {
        ThrottleAction?.action.Enable();
        BrakeAction?.action.Enable();
        SteeringAction?.action.Enable();
        DriftAction?.action.Enable();

        if (carRenderer != null)
        {
            _carMaterial = carRenderer.material;
            _originalColor = _carMaterial.color;
        }
    }

    private void OnEnable()
    {
        if (PlayerInput != null)
        {
            PlayerInput.onActionTriggered += OnActionTriggerd;
        }
    }

    private void OnDisable()
    {
        if (PlayerInput != null)
        {
            PlayerInput.onActionTriggered -= OnActionTriggerd;
        }
    }

    private void Update()
    {
        // Color transition while drifting
        if (_carMaterial != null)
        {
            Color targetColor = _drift ? driftColor : _originalColor;
            _carMaterial.color = Color.Lerp(_carMaterial.color, targetColor, Time.deltaTime * colorLerpSpeed);
        }

        if (driftParticles != null)
        {
            if (_drift && !driftParticles.isPlaying)
            {
                driftParticles.Play();
                driftParticles2.Play();
            }
            else if (!_drift && driftParticles.isPlaying)
            {
                driftParticles.Stop();
                driftParticles2.Stop();
            }
        }
    }

    private void OnActionTriggerd(InputAction.CallbackContext context)
    {
        if (context.action == ThrottleAction?.action)
        {
            _throttle = context.ReadValue<float>();
        }
        else if (context.action == BrakeAction?.action)
        {
            _brake = context.ReadValue<float>();
        }
        else if (context.action == SteeringAction?.action)
        {
            _steering = context.ReadValue<float>();
        }
        else if (context.action == DriftAction?.action)
        {
            _drift = context.ReadValueAsButton();
        }
    }

    public override float GetThrottle() => _throttle;
    public override float GetBrake() => _brake;
    public override float GetSteering() => _steering;
    public override bool GetDrift() => _drift;
}

using UnityEngine;

public class KartCamera : MonoBehaviour
{
    public Carcontroller Kart;

    [Header("Offset Settings")]
    public Vector3 OffsetPosition = new Vector3(0f, 2f, -5f);
    public Vector3 OffsetRotation = new Vector3(0f, 0f, 0f);

    [Header("Drift Camera Offset")]
    [SerializeField] private Vector3 normalOffset = new Vector3(0f, 3f, -5.5f);
    [SerializeField] private Vector3 driftOffset = new Vector3(0f, 3f, -8f);
    [SerializeField] private float offsetLerpSpeed = 5f;

    [Header("Chase Settings")]
    public float ChaseDistance = 1f;
    [Range(0, 1)] public float KartFitRatio = 0.5f;
    public float TiltAngleSmoothing = 1;

    private Vector3 _chacingPivot;
    private Vector3 _smoothingGroundNormal;
    private Vector3 _lastCameraUpward;

    private void Start()
    {
        _chacingPivot = Kart.transform.position - Kart.transform.forward * ChaseDistance;
        _smoothingGroundNormal = Vector3.up;
        _lastCameraUpward = Vector3.up;
    }

    private void LateUpdate()
    {
        // Smooth LERP between drift and normal camera positions
        Vector3 targetOffset = Kart.isDrifting ? driftOffset : normalOffset;
        OffsetPosition = Vector3.Lerp(OffsetPosition, targetOffset, Time.deltaTime * offsetLerpSpeed);

        var worldUpward = Vector3.up;

        _chacingPivot = Kart.transform.position + (_chacingPivot - Kart.transform.position).normalized * ChaseDistance;

        // Ground normal smoothing
        if (Kart.IsGrounded)
        {
            var smoothingGroundNormalAngle = Vector3.Angle(_smoothingGroundNormal, Kart.GroundNormal);
            var cameraUpwardAngle = Vector3.Angle(_lastCameraUpward, Kart.GroundNormal);
            if (smoothingGroundNormalAngle > cameraUpwardAngle)
            {
                _smoothingGroundNormal = _lastCameraUpward;
            }
        }

        // Horizontal rotation based on chase target
        var moveForward = (Kart.transform.position - _chacingPivot).normalized;
        var mergedForward = Vector3.Lerp(moveForward, Kart.transform.forward, KartFitRatio);
        var mergedForwardHorizontal = mergedForward - Vector3.Project(mergedForward, worldUpward);
        var rot = Quaternion.LookRotation(mergedForwardHorizontal);

        // Tilt rotation
        var moveUpward = Vector3.Cross(moveForward, Vector3.Cross(worldUpward, moveForward));
        var targetGroundNormal = Kart.IsGrounded ? Kart.GroundNormal : moveUpward;
        _smoothingGroundNormal = Vector3.Slerp(_smoothingGroundNormal, targetGroundNormal, TiltAngleSmoothing * Time.deltaTime);

        var groundForward = Vector3.Cross(rot * Vector3.right, _smoothingGroundNormal);
        var groundForwardHorizontal = groundForward - Vector3.Project(groundForward, worldUpward);
        var tiltRot = Quaternion.LookRotation(new Vector3(0, Vector3.Dot(groundForward, worldUpward), groundForwardHorizontal.magnitude));
        rot = rot * tiltRot;

        // Final position and rotation
        transform.rotation = rot * Quaternion.Euler(OffsetRotation);
        transform.position = Kart.transform.position + rot * OffsetPosition;

        _lastCameraUpward = rot * Vector3.up;
    }
}

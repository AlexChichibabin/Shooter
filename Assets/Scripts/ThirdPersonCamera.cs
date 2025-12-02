using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    //[SerializeField] private Camera camera;
    [SerializeField] private Transform aim;
    [SerializeField] private float sensitivity; 
    [SerializeField] private float rotateTargetLerpRate;

    [Header("Distance")]
    [SerializeField] private float distance;
    [SerializeField] private float minDistance;
    [SerializeField] private float distanceLerpRate;
    [SerializeField] private float distanceOffsetFromCollisionHit;

    [Header("Fov")]
    [SerializeField] private float normalFov = 60;
    [SerializeField] private float aimingFov;
    [SerializeField] private float fovLerpRate;

    [Header("Rotation Limit")]
    [SerializeField] private float MaxLimitY;
    [SerializeField] private float MinLimitY;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float m_OffsetAccelerationRate;

    [HideInInspector] public bool IsRotateTarget;
    [HideInInspector] public Vector2 RotationControl;

    private float deltaRotationX;
    private float deltaRotationY;

    private float currentDistance;

    private Vector3 TargetOffset;
    private Vector3 DefaultOffset;

    private float targetFov;

    public Transform Aim => aim;

    private void Start()
    {
        DefaultOffset = offset;
        TargetOffset = offset;
        targetFov = normalFov;

        transform.SetParent(null);
    }
    private void Update() // Ѕыл просто Update. ”шли фризы анимации перса
    {
        if (target == null)
        {
            if (Player.Instance == null) return;
            target = Player.Instance.transform;
        }

        // Calculate rotation and translation
        deltaRotationX += RotationControl.x * sensitivity;
        deltaRotationY += RotationControl.y * sensitivity;

        deltaRotationY = ClampAngle(deltaRotationY, MinLimitY, MaxLimitY);

        offset = Vector3.MoveTowards(offset, TargetOffset, Time.deltaTime * m_OffsetAccelerationRate);

        Quaternion finalRotation = Quaternion.Euler(-deltaRotationY, deltaRotationX, 0);

        Vector3 finalPosition = target.position - (finalRotation * Vector3.forward * distance);
        finalPosition = AddLocalOffset(finalPosition);

        // Calculate current distance
        float targetDistance = distance;

        RaycastHit hit;
        Debug.DrawLine(target.position + new Vector3(0, offset.y, 0), finalPosition, Color.yellow);

        if (Physics.Linecast(target.position + new Vector3(0, offset.y, 0), finalPosition, out hit) == true)
        {
            if (hit.collider.isTrigger == false)
            {
                float distanceToHit = Vector3.Distance(target.position + new Vector3(0, offset.y, 0), hit.point);

                if (hit.transform != target)
                {
                    if (distanceToHit < distance)
                        targetDistance = distanceToHit - distanceOffsetFromCollisionHit;
                }
            }
        }
        currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, Time.deltaTime * distanceLerpRate);
        currentDistance = Mathf.Clamp(currentDistance, minDistance, distance);

        // Correct camera position
            finalPosition = target.position - (finalRotation * Vector3.forward * currentDistance);

        // Apply transform
        transform.rotation = finalRotation; //
        transform.position = finalPosition; //
        transform.position = AddLocalOffset(transform.position);

        Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, targetFov, Time.deltaTime * fovLerpRate);

        // Rotation target
        if (IsRotateTarget == true)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, Time.deltaTime * rotateTargetLerpRate);
        }
            
    }
    public void SetTarget(Transform target) => this.target = target;
    public void SetTargetOffset(Vector3 offset) => TargetOffset = offset;
    public void SetDefaultOffset() => TargetOffset = DefaultOffset;
    public void SetFov(bool isAiming) => targetFov = isAiming ? aimingFov : normalFov;

    private Vector3 AddLocalOffset(Vector3 position)
    {
        Vector3 result = position;
        result += new Vector3(0, offset.y, 0);
        result += transform.right * offset.x;
        result += transform.forward * offset.z;

        return result;
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 360f) angle -= 360f;
        if (angle < -360) angle += 360f;

        return Mathf.Clamp(angle, min, max);
    }
}

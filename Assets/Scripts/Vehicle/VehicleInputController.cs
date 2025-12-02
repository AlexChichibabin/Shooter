using UnityEngine;

public class VehicleInputController : MonoBehaviour
{
    [SerializeField] private Vehicle m_Vehicle;
    [SerializeField] private ThirdPersonCamera m_Camera;
    [SerializeField] private Turret m_Turret;
    [SerializeField] private Vector3 m_CameraOffset;


    protected virtual void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (m_Camera != null)
        {
            m_Camera.IsRotateTarget = false;
            m_Camera.SetTargetOffset(m_CameraOffset);
        }
    }
    protected virtual void Update()
    {
        m_Vehicle.SetTargetControl(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical")));
        m_Camera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetMouseButton(0))
        {
            if (m_Turret == null) return;
            m_Turret.Fire();
        }

    }
    public void AssignCamera(ThirdPersonCamera camera)
    {
        m_Camera = camera;
        m_Camera.IsRotateTarget = false;
        m_Camera.SetTargetOffset(m_CameraOffset);
        m_Camera.SetTarget(m_Vehicle.transform);
    }
}

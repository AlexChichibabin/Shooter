using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_TargetCharacterMovement;
    [SerializeField] private ThirdPersonCamera m_TargetCamera;
    [SerializeField] private PlayerShooter m_TargetShooter;
    [SerializeField] private SpreadShootRig m_SpreadShootRig;

    [SerializeField] private Vector3 m_AimingOffset;

    [SerializeField] private Vector3 DefaultOffset;

    private void Start()
    {
        LockMouse();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnLockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        m_TargetCharacterMovement.TargetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_TargetCamera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (m_TargetCharacterMovement.TargetDirectionControl != Vector3.zero || m_TargetCharacterMovement.IsAiming == true)
        {
            m_TargetCamera.IsRotateTarget = true;
        }
        else m_TargetCamera.IsRotateTarget = false;

        if (Input.GetMouseButton(0) && m_TargetCharacterMovement.IsAiming == true)
        {
            m_TargetShooter.Shoot();
            m_SpreadShootRig.Spread(true);
        }
        else m_SpreadShootRig.Spread(false);

        if (Input.GetButtonDown("Jump"))
            m_TargetCharacterMovement.Jump();

        if (Input.GetKey(KeyCode.LeftControl)) m_TargetCharacterMovement.Crouch();
        else m_TargetCharacterMovement.UnCrouch();

        if (Input.GetKey(KeyCode.LeftShift)) m_TargetCharacterMovement.Sprint();
        if (Input.GetKeyUp(KeyCode.LeftShift)) m_TargetCharacterMovement.UnSprint();

        if (Input.GetMouseButton(1))
        {
            m_TargetCharacterMovement.Aiming();
            m_TargetCamera.SetTargetOffset(m_AimingOffset);
            m_TargetCamera.SetFov(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            m_TargetCharacterMovement.UnAiming();
            m_TargetCamera.SetDefaultOffset();
            m_TargetCamera.SetFov(false);
        }
    }
    public void AssignCamera(ThirdPersonCamera camera)
    {
        m_TargetCamera = camera;
        m_TargetCamera.IsRotateTarget = false;
        m_TargetCamera.SetTargetOffset(DefaultOffset);
        m_TargetCamera.SetTarget(m_TargetCharacterMovement.transform);
    }
}

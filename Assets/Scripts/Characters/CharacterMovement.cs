using UnityEngine;
using UnityEngine.Events;

public class CharacterMovement : MonoBehaviour
{
    public UnityAction<Vector3> Land;

    [SerializeField] private CharacterController m_CharacterController;


    [Header("Movement")]
    [SerializeField] private float m_RifleRunSpeed;
    [SerializeField] private float m_RifleSprintSpeed;
    [SerializeField] private float m_AimingWalkSpeed;
    [SerializeField] private float m_AimingRunSpeed;
    [SerializeField] private float m_CrouchSpeed;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_AccelerationRate;
    [SerializeField] private float m_GroundBaseHeight = 0.005f;

    [Header("State")]
    [SerializeField] private float m_CrouchHeight;

    private bool isAiming;
    private bool isJump;
    private bool isJumpForAnimation;
    private bool isCrouch;
    private bool isSprint;
    private bool isClimbing;
    private float distanceToGround;

    public float NoiseLevel => isCrouch? m_CharacterController.velocity.magnitude - 2 : m_CharacterController.velocity.magnitude;
    public float JumpSpeed => m_JumpSpeed;
    public float CrouchHeight => m_CrouchHeight;
    public bool IsAiming => isAiming;
    public bool IsJump => isJump;
    public bool IsJumpForAnimation => isJumpForAnimation;
    public bool IsCrouch => isCrouch;
    public bool IsSprint => isSprint;
    public bool IsClimbing => isClimbing;
    public float DistanceToGround => distanceToGround;
    public bool IsGrounded => distanceToGround < m_GroundBaseHeight || m_CharacterController.isGrounded;
    public float CurrentSpeed => GetCurrentSpeepByState();
    public bool UpdatePosition;

    private float m_BaseCharacterHeight;
    private float m_BaseCharacterHeightOffset;

    [HideInInspector] public Vector3 TargetDirectionControl;
    [HideInInspector] public Vector3 DirectionControl;
    private Vector3 movementDirection;

    private void Start()
    {
        m_BaseCharacterHeight = m_CharacterController.height;
        m_BaseCharacterHeightOffset = m_CharacterController.center.y;
    }
    private void Update()
    {
        UpdateDistanceToGround();
        //if(distanceToGround > m_GroundBaseHeight) Debug.Log(distanceToGround);
        Move();
    }

    public void Jump()
    {
        if (distanceToGround > 0.1f && IsGrounded == false) return; // Решение проблемы с прыжками
        isJump = true;
        isJumpForAnimation = true;
    }
    public void JumpForAnimToFalse() => isJumpForAnimation = false;
    public void Crouch()
    {
        if (IsGrounded == false) return;
        if (isSprint == true) return;

        isCrouch = true;
        m_CharacterController.height = m_CrouchHeight;
        m_CharacterController.center = new Vector3(0, m_CharacterController.height / 2, 0); // Не середина Height ли? Или m_BaseCharacterHeightOffset / 2
    }
    public void UnCrouch()
    {
        isCrouch = false;

        m_CharacterController.height = m_BaseCharacterHeight;
        m_CharacterController.center = new Vector3(0, m_BaseCharacterHeightOffset, 0);
    }
    public void Sprint()
    {
        if (IsGrounded == false) return;
        isSprint = true;
    }
    public void UnSprint()
    {
        isSprint = false;
    }
    public void Aiming()
    {
        if (IsGrounded == false) return;

        isAiming = true;
    }
    public void UnAiming()
    {
        isAiming = false;
    }
    public void SetClimbing(bool isClimbing) => this.isClimbing = isClimbing;

    //Private
    public float GetCurrentSpeepByState()
    {
        if(isCrouch == true) return m_CrouchSpeed;
        if (isAiming == true)
        {
            if (isSprint == true) return m_AimingRunSpeed;
            else return m_AimingWalkSpeed;
        }
        else
        {
            if (isSprint == true) return m_RifleSprintSpeed;
            else return m_RifleRunSpeed;
        }
    }
    public void Stop()
    {
        m_CharacterController?.Move(new Vector3(0,0,0));
    }
    private void Move()
    {
        DirectionControl = Vector3.MoveTowards(DirectionControl, TargetDirectionControl.normalized *
            Mathf.Clamp(TargetDirectionControl.magnitude, 0, 1), Time.deltaTime * m_AccelerationRate);

        if (IsGrounded == true)
        {
            movementDirection = DirectionControl * GetCurrentSpeepByState();

            if (isJump == true)
            {
                movementDirection.y = JumpSpeed;
                isJump = false;
            }

            movementDirection = transform.TransformDirection(movementDirection);
        }
        
        movementDirection += Physics.gravity * Time.deltaTime;

        if (UpdatePosition == true)
            m_CharacterController?.Move(movementDirection * Time.deltaTime);

        if (m_CharacterController.isGrounded == true && Mathf.Abs(movementDirection.y) > 2)
        {
            if (Land != null) Land.Invoke(movementDirection);
        }
    }
    private void UpdateDistanceToGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if (Physics.Raycast(ray, out hit) == true)
        {
            distanceToGround = hit.distance;
        }   
    }
}

using System;
using UnityEngine;

[Serializable]
public class CharacterAnimationParameterNames
{
    public string NormalizeMovementX;
    public string NormalizeMovementZ;
    public string Sprint;
    public string Crouch;
    public string Aiming;
    public string Ground;
    public string Jump;
    public string GroundSpeed;
    public string DistanceToGround;
    public string Climb;
}
[Serializable]
public class AnimationCrossFadeParameters
{
    public string Name;
    public float Duration;
}

public class CharacterAnimationState : MonoBehaviour
{
    private const float INPUT_CONTROL_LERP_RATE = 10.0f;

    [SerializeField] private CharacterController m_TargetCharacterController;
    [SerializeField] private CharacterMovement m_TargetCharacterMovement;
    [SerializeField] private Animator m_TargetAnimator;

    [SerializeField]
    [Space(5)] private CharacterAnimationParameterNames m_ParameterNames;

    [Header("Fades")]
    [Space(5)][SerializeField] private AnimationCrossFadeParameters m_FallFade;
    [SerializeField] private float MinDistanceToGroundByFall;
    [SerializeField] private AnimationCrossFadeParameters m_JumpIdleFade;
    [SerializeField] private AnimationCrossFadeParameters m_JumpMoveFade;

    public AnimationCrossFadeParameters FallFade => m_FallFade;

    private Vector3 inputControl;


    private void LateUpdate()
    {
        Vector3 movementSpeed = transform.InverseTransformDirection(m_TargetCharacterController.velocity);

        inputControl = Vector3.MoveTowards(inputControl, m_TargetCharacterMovement.TargetDirectionControl, Time.deltaTime * INPUT_CONTROL_LERP_RATE);

        m_TargetAnimator.SetFloat(m_ParameterNames.NormalizeMovementX, inputControl.x);
        m_TargetAnimator.SetFloat(m_ParameterNames.NormalizeMovementZ, inputControl.z);

        m_TargetAnimator.SetBool(m_ParameterNames.Crouch, m_TargetCharacterMovement.IsCrouch);
        m_TargetAnimator.SetBool(m_ParameterNames.Sprint, m_TargetCharacterMovement.IsSprint);
        m_TargetAnimator.SetBool(m_ParameterNames.Aiming, m_TargetCharacterMovement.IsAiming);
        m_TargetAnimator.SetBool(m_ParameterNames.Ground, m_TargetCharacterMovement.IsGrounded);
        m_TargetAnimator.SetBool(m_ParameterNames.Climb, m_TargetCharacterMovement.IsClimbing);

        Vector3 groundSpeed = m_TargetCharacterController.velocity;
        groundSpeed.y = 0;
        m_TargetAnimator.SetFloat(m_ParameterNames.GroundSpeed, groundSpeed.magnitude);

        if (m_TargetCharacterMovement.IsJumpForAnimation == true)
        {
            m_TargetCharacterMovement.JumpForAnimToFalse();
            if (groundSpeed.magnitude <= 0.01f) CrossFade(m_JumpIdleFade);
            if (groundSpeed.magnitude > 0.01f) CrossFade(m_JumpMoveFade);
        }
        if (m_TargetCharacterMovement.IsGrounded == false)
        {
            m_TargetAnimator.SetFloat(m_ParameterNames.Jump, movementSpeed.y);

            if (movementSpeed.y < 0 && m_TargetCharacterMovement.DistanceToGround > MinDistanceToGroundByFall)
                CrossFade(m_FallFade);
        }

        m_TargetAnimator.SetFloat(m_ParameterNames.DistanceToGround, m_TargetCharacterMovement.DistanceToGround);
    }
    private void CrossFade(AnimationCrossFadeParameters parameters)
    {
        m_TargetAnimator.CrossFade(parameters.Name, parameters.Duration);
    }
}

using UnityEngine;

public class AimingRig : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_TargetCharacter;
    [SerializeField] private UnityEngine.Animations.Rigging.Rig[] m_Rigs;
    [SerializeField] private float m_ChangeWeightLerpRate;

    private float targetWeight;

    private void Update()
    {
        if (m_TargetCharacter.IsAiming == true) targetWeight = 1f;
        else targetWeight = 0f;

        for (int i = 0; i < m_Rigs.Length; ++i)
        {
            m_Rigs[i].weight = Mathf.MoveTowards(m_Rigs[i].weight, targetWeight, Time.deltaTime * m_ChangeWeightLerpRate);
        }
    }
}

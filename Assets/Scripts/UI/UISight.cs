using UnityEngine;
using UnityEngine.UI;

public class UISight : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private Image m_ImageSight;

    private void Update()
    {
        m_ImageSight.enabled = m_CharacterMovement.IsAiming;
    }   
}

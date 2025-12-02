using UnityEngine;
using UnityEngine.UI;

public class UIWeaponEnergy : MonoBehaviour
{
    [SerializeField] private Weapon m_TargetWeapon;

    [SerializeField] private Slider m_Slider;

    [SerializeField] private Image m_CurrentEnergyImage;

    [SerializeField] private Image[] Images;

    private Color defaultEnergyColor;

    private void Start()
    {
        defaultEnergyColor = m_CurrentEnergyImage.color;
        m_Slider.maxValue = m_TargetWeapon.PrimaryMaxEnergy;
    }
    private void FixedUpdate()
    {
        m_Slider.value = m_TargetWeapon.PrimaryEnergy;
        if (m_TargetWeapon.EnergyIsRestored == true) m_CurrentEnergyImage.color = Color.red;
        else m_CurrentEnergyImage.color = defaultEnergyColor;

        SetActiveImages(m_TargetWeapon.PrimaryEnergy != m_TargetWeapon.PrimaryMaxEnergy);
    }
    private void SetActiveImages(bool isActive)
    {
        for (int i = 0; i < Images.Length; i++)
        {
            Images[i].enabled = isActive;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UIHitPointSlider : MonoBehaviour
{
    [SerializeField] private Destructible m_Destructible;

    [SerializeField] private Slider m_Slider;

    [SerializeField] private Image m_CurrentHitImage;

    [SerializeField] private Image[] Images;

    private Color defaultEnergyColor;

    private void Start()
    {
        defaultEnergyColor = m_CurrentHitImage.color;
        m_Slider.maxValue = m_Destructible.HitPoints;
    }
    private void Update()
    {
        m_Slider.value = m_Destructible.CurrentHitPoints;
        /*if (m_TargetDestructible.EnergyIsRestored == true) m_CurrentHitImage.color = Color.red;
        else m_CurrentHitImage.color = defaultEnergyColor;*/

        //SetActiveImages(m_TargetWeapon.PrimaryEnergy != m_TargetWeapon.PrimaryMaxEnergy);
    }
    private void SetActiveImages(bool isActive)
    {
        for (int i = 0; i < Images.Length; i++)
        {
            Images[i].enabled = isActive;
        }
    }
}

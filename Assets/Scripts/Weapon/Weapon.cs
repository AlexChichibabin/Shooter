using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Parameters
    [SerializeField] private WeaponMode m_Mode;
    public WeaponMode Mode => m_Mode;

    [SerializeField] private float m_FireDelay;
    [SerializeField] private float m_PrimaryMaxEnergy;
    [SerializeField] private WeaponProperties m_WeaponProperties;
    [SerializeField] private Transform m_FireSource;
    [SerializeField] private NoiseAudioSource m_NoiseAudioSource;
    [SerializeField] private ParticleSystem m_MuzzleParticleSystem;
    [SerializeField] private Light m_Light;

    private float m_RefireTimer;
    private float m_LightTimer;
    private float m_PrimaryEnergy;
    private bool m_EnergyIsRestored;
    private Destructible m_Owner;
    public bool CanFire => m_RefireTimer <= 0 && m_EnergyIsRestored == false;
    public float PrimaryMaxEnergy => m_PrimaryMaxEnergy;
    public float PrimaryEnergy => m_PrimaryEnergy;
    public bool EnergyIsRestored => m_EnergyIsRestored;

    #endregion

    #region UnityEvents
    private void Start()
    {
        m_PrimaryEnergy = m_PrimaryMaxEnergy;
        m_Owner = transform.root.GetComponent<Destructible>();
        if (m_Light) m_Light.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (m_RefireTimer > 0) m_RefireTimer -= Time.deltaTime;
        if (m_LightTimer > 0) m_LightTimer -= Time.deltaTime;
        else if (m_Light) m_Light.gameObject.SetActive(false);

        UpdateEnergy();
    }
    #endregion

    #region PublicAPI
    public void Fire()
    {
        if (m_WeaponProperties == null) return;
        if (m_RefireTimer > 0) return;
        if (CanFire == false) return;
        if (TryDrawEnergy(m_WeaponProperties.EnergyUsage) == false) return;

        Projectile projectile = Instantiate(m_WeaponProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = m_FireSource.position;
        projectile.transform.forward = m_FireSource.forward;

        projectile.SetParentShooter(m_Owner);

        m_RefireTimer = m_WeaponProperties.RateOfFire;

        m_NoiseAudioSource.audioClip = m_WeaponProperties.LaunchSFX;
        m_NoiseAudioSource.Play();
        if (m_MuzzleParticleSystem != null) m_MuzzleParticleSystem.Play();
        if (m_Light != null) m_Light.gameObject.SetActive(true);
    }

    public void AssignLoadout(WeaponProperties props)
    {
        if (m_Mode != props.Mode) return;

        m_RefireTimer = 0;
        m_WeaponProperties = props;
    }
    public void FirePointLookAt(Vector3 pos)
    {
        Vector3 offset = Random.insideUnitSphere * m_WeaponProperties.SpreadShootRange;

        if (m_WeaponProperties.SpreadShootDistanceFactor != 0)
        {
            offset = offset * Vector3.Distance(m_FireSource.position, pos) * m_WeaponProperties.SpreadShootDistanceFactor;
        }

        m_FireSource.LookAt(pos + offset);
    }
    #endregion

    private bool TryDrawEnergy(int count)
    {
        if (count == 0) return true;

        if (m_PrimaryEnergy >= count)
        {
            m_PrimaryEnergy -= count;
            return true;
        }
        m_EnergyIsRestored = true;
        return false;
    }
    private void UpdateEnergy()
    {
        m_PrimaryEnergy += (float)m_WeaponProperties.EnergyRegenPerSecond * Time.fixedDeltaTime;
        m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_PrimaryMaxEnergy);

        if (m_PrimaryEnergy >= m_WeaponProperties.EnergyAmountToStartFire) m_EnergyIsRestored = false;
    }
}

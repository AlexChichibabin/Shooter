using UnityEngine;

public enum WeaponMode
{
    Primary,
    Secondary
}

[CreateAssetMenu]
public sealed class WeaponProperties : ScriptableObject
{
    [SerializeField] private WeaponMode m_Mode;
    public WeaponMode Mode => m_Mode;

    [SerializeField] private Projectile m_ProjectilePrefab;
    public Projectile ProjectilePrefab => m_ProjectilePrefab;

    [SerializeField] private float m_SpreadShootRange;
    public float SpreadShootRange => m_SpreadShootRange;

    [SerializeField] private float m_SpreadShootDistanceFactor;
    public float SpreadShootDistanceFactor => m_SpreadShootDistanceFactor;

    [SerializeField] private float m_RateOfFire;
    public float RateOfFire => m_RateOfFire;

    [SerializeField] private int m_EnergyUsage;
    public int EnergyUsage => m_EnergyUsage;

    [SerializeField] private int m_AmmoUsage;
    public int AmmoUsage => m_AmmoUsage;

    [SerializeField] private int m_EnergyAmountToStartFire;
    public int EnergyAmountToStartFire => m_EnergyAmountToStartFire;

    [SerializeField] private float m_EnergyRegenPerSecond;
    public float EnergyRegenPerSecond => m_EnergyRegenPerSecond;

    [SerializeField] private AudioClip m_LaunchSFX;
    public AudioClip LaunchSFX => m_LaunchSFX;
}

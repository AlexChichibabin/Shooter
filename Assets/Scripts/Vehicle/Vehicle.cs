using System.Collections;
using UnityEngine;

public class Vehicle : Destructible
{
    [SerializeField] protected float m_MaxLinearVelocity;

    public virtual float LinearVelocity => 0;

    private Destructible driver;
    public Destructible Driver => driver;

    public float NormalizedLinearVelocity
    {
        get
        {
            if (Mathf.Approximately(0, LinearVelocity) == true) return 0;

            return Mathf.Clamp01(LinearVelocity / m_MaxLinearVelocity);
        }
    }

    protected Vector3 TargetInputControl;

    public void SetDriver(Destructible driver) => this.driver = driver; 
    public void SetTargetControl(Vector3 control)
    {
        TargetInputControl = control.normalized;
    }

    [Header("EngineSound")]
    [SerializeField] private AudioSource m_EngineSFX;
    [SerializeField] private float m_TargetSFXVolumeBase = 0.4f;
    [SerializeField] private float m_EngineSFXModifier;
    [SerializeField] private float m_EngineSFXVolumeLerpRate;

    private float defaultSFXVolumeBase;
    public float DefaultSFXVolumeBase => defaultSFXVolumeBase;

    protected override void Start()
    {
        base.Start();
        defaultSFXVolumeBase = m_TargetSFXVolumeBase;
        SetVolume(0);
    }
    protected virtual void Update()
    {
        UpdateEngineSFX();
    }

    private void UpdateEngineSFX()
    {
        if (m_EngineSFX != null)
        {
            m_EngineSFX.pitch = 1.0f + NormalizedLinearVelocity * m_EngineSFXModifier;
            m_EngineSFX.volume = m_TargetSFXVolumeBase + NormalizedLinearVelocity; // Дописать
        }
    }
    public void SetVolume(float targetVolume)
    {
        StartCoroutine(SetSFXVolumeStateNumerator(targetVolume));
    }
    IEnumerator SetSFXVolumeStateNumerator(float targetVolume)
    {
        while (Mathf.Approximately(m_TargetSFXVolumeBase, targetVolume) != true)
        {
            m_TargetSFXVolumeBase = Mathf.MoveTowards(m_TargetSFXVolumeBase, targetVolume, Time.deltaTime * m_EngineSFXVolumeLerpRate);
            yield return new WaitForEndOfFrame();
        }
        if(m_EngineSFX.isPlaying == false) m_EngineSFX.Play();
    }
}

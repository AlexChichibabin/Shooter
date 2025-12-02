using UnityEngine;

[System.Serializable]
public class FootStepProperties
{
    public float Speed;
    public float Delay;
}

public class FootStepSound : MonoBehaviour
{
    [SerializeField] private FootStepProperties[] m_Properties;
    [SerializeField] private CharacterController m_CharacterController;
    [SerializeField] private NoiseAudioSource m_NoiseAudioSource;
    [SerializeField] private so_Sounds m_so_Sounds;

    private float Delay;
    private float Tick;

    private void Update()
    {
        if (IsPlay() == false)
        {
            Tick = 0;
            return;
        }
            
        Tick += Time.deltaTime;
        Delay = GetDelayBySpeed(GetSpeed());

        if (Tick >= Delay)
        {
            m_NoiseAudioSource.audioClip = m_so_Sounds.GetRandomAudio();
            m_NoiseAudioSource.Play();
            Tick = 0;
        }
    }

    private bool IsPlay()
    {
        if (GetSpeed() <= 0.01f || m_CharacterController.isGrounded == false) return false;
        return true;

    }
    private float GetSpeed()
    {
        return m_CharacterController.velocity.magnitude;
    }
    private float GetDelayBySpeed(float speed)
    {
        for (int i = 0; i < m_Properties.Length; i++)
        {
            if (speed <= m_Properties[i].Speed)
            {
                return m_Properties[i].Delay;
            }
        }
        return m_Properties[m_Properties.Length - 1].Delay;
    }
}

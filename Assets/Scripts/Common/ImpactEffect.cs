using UnityEngine;

public enum ImpactType
{
    NoDecal,
    WithDecals
}
public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float m_LifeTime;

    [SerializeField] private GameObject m_Decal;

    private float m_Timer;

    private void Update()
    {
        if (m_Timer < m_LifeTime)
        {
            m_Timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateType(ImpactType type)
    {
        if (type == ImpactType.NoDecal)
        {
            m_Decal.SetActive(false);
        }
    }
}


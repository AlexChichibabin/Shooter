using UnityEngine;
using UnityEngine.Events;

public abstract class EntityActionProperties { }

public abstract class EntityAction : MonoBehaviour
{
    [SerializeField] private UnityEvent m_EventOnStart;
    [SerializeField] private UnityEvent m_EventOnEnd;

    private EntityActionProperties m_Properties;
    private bool m_IsStarted;

    public EntityActionProperties Properties => m_Properties;
    public UnityEvent EventOnStart => m_EventOnStart;
    public UnityEvent EventOnEnd => m_EventOnEnd;

    public virtual void StartAction()
    {
        if (m_IsStarted == true) return;
        m_IsStarted = true;

        m_EventOnStart?.Invoke();
    }
    public virtual void EndAction()
    {
        m_IsStarted = false;

        m_EventOnEnd?.Invoke();
    }
    public virtual void SetProperties(EntityActionProperties props)
    {
        m_Properties = props;
    }
}

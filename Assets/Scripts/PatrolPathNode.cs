using UnityEngine;

public class PatrolPathNode : MonoBehaviour
{
    [SerializeField] private float m_IdleTime;
    public float IdleTime => m_IdleTime;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
#endif
}

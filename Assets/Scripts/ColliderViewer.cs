using UnityEngine;

public class ColliderViewer : MonoBehaviour
{
    [SerializeField] private float m_ViewAngle;
    [SerializeField] private float m_ViewDistance;
    [SerializeField] private float m_ViewHeight;

    // Public API
    public bool IsObjectVisible(GameObject target)
    {
        ColliderViewPoints viewPoints = target.GetComponent<ColliderViewPoints>();

        if (viewPoints == null) return false;
        
        return viewPoints.IsVisibleFromPoint(transform.position + new Vector3(0, m_ViewHeight, 0), transform.forward, m_ViewAngle, m_ViewDistance);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, m_ViewHeight, 0), transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, m_ViewAngle, 0, m_ViewDistance, 1);
    }
#endif
}

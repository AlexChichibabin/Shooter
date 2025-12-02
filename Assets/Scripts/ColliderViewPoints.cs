using UnityEngine;

public class ColliderViewPoints : MonoBehaviour
{
    private enum ColliderType
    {
        Character
    }
    [SerializeField] private ColliderType m_ColliderType;
    [SerializeField] private Collider m_Collider;
    [SerializeField] private Vector3[] m_Points;

    private void Start()
    {
        if (m_ColliderType == ColliderType.Character)
        {
            UpdatePointsForCharacterController();
        }
    }
    private void Update()
    {
        if (m_ColliderType == ColliderType.Character)
        {
            CalcPointsForCharacterController(m_Collider as CharacterController);
        }
    }

    // Public API
    public bool IsVisibleFromPoint(Vector3 point, Vector3 eyeDir, float viewAngle, float viewDistance)
    {
        for (int i = 0; i < m_Points.Length; i++)
        {
            
            float angle = Vector3.Angle(m_Points[i] - point, eyeDir);
            float distance = Vector3.Distance(m_Points[i], point);
            
            if (angle <= viewAngle * 0.5f && distance <= viewDistance)
            {
                RaycastHit hit;
                /*Debug.Log($"{i}");
                Debug.Log($"{angle <= viewAngle * 0.5f} + {distance <= viewDistance}, " +
                    $"{Physics.Raycast(point, (m_Points[i] - point).normalized, out hit, viewDistance * 2) == true}");
                Debug.Log(hit.collider.gameObject);*/

                Debug.DrawLine(point, m_Points[i], Color.blue);
                if (Physics.Raycast(point, (m_Points[i] - point).normalized, out hit, viewDistance * 2) == true)
                {
                    if (hit.collider == m_Collider)
                    {
                        //Debug.Log("return true");
                        return true;
                    }
                }

            }
        }
        //Debug.Log("return false");
        return false;
        
    }

    [ContextMenu("UpdateViewPoints")]
    public void UpdateViewPoints()
    {
        if (m_Collider == null) return;

        m_Points = null;

        if (m_ColliderType == ColliderType.Character)
        {
            UpdatePointsForCharacterController();
        }
    }
    private void UpdatePointsForCharacterController()
    {
        if (m_Points == null)
        {
            m_Points = new Vector3[4];
        }

        CharacterController collider = m_Collider as CharacterController;

        CalcPointsForCharacterController(collider);
    }
    private void CalcPointsForCharacterController(CharacterController collider)
    {
        m_Points[0] = collider.transform.position + collider.center + collider.transform.up * collider.height * 0.3f;
        m_Points[1] = collider.transform.position + collider.center - collider.transform.up * collider.height * 0.3f;
        m_Points[2] = collider.transform.position + collider.center + collider.transform.right * collider.radius * 0.4f;
        m_Points[3] = collider.transform.position + collider.center - collider.transform.right * collider.radius * 0.4f;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (m_Points != null)
        {
            for (int i = 0; i < m_Points.Length; i++)
            {
                Gizmos.DrawSphere(m_Points[i], 0.05f);
            }
        }
    }
#endif
}

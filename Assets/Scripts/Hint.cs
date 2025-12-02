using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject m_Hint;
    [SerializeField] private float m_ActiveRadius;

    private Canvas m_Canvas;
    private Transform m_Target;
    private Transform m_LookTransform;

    private void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        if (Camera.main != null)
        {
            m_Canvas.worldCamera = Camera.main;
            m_LookTransform = Camera.main.transform;
        }
        //m_Target = Player.Instance.transform;
    }
    private void Update()
    {
        if (m_Target == null)
        {
            if(Player.Instance != null) m_Target = Player.Instance.transform;
            if (m_Target == null) return;
        }
        m_Hint.transform.LookAt(m_LookTransform);

        if (Vector3.Distance(transform.position, m_Target.position) < m_ActiveRadius)
        {
            m_Hint.SetActive(true);
        }
        else
            m_Hint.SetActive(false);
    }

/*#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_ActiveRadius);  
    }
#endif*/
}
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private PatrolPathNode[] m_Nodes;

    private void Start()
    {
        UpdatePathNode();
    }
    public PatrolPathNode GetRandomPathNode()
    {
        return m_Nodes[Random.Range(0, m_Nodes.Length)];
    }
    public PatrolPathNode GetNextPathNode(ref int index)
    {
        index = Mathf.Clamp(index, 0, m_Nodes.Length - 1);

        index++;

        if (index >= m_Nodes.Length) index = 0;

        return m_Nodes[index];
    }
    [ContextMenu("Update Path Node")]
    private void UpdatePathNode()
    {
        m_Nodes = new PatrolPathNode[transform.childCount];

        for (int i = 0; i < m_Nodes.Length; i++)
        {
            m_Nodes[i] = transform.GetChild(i).GetComponent<PatrolPathNode>();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (m_Nodes == null) return;
        if (m_Nodes.Length == 0) return;
        for (int i = 0; i < m_Nodes.Length - 1; i++)
        {
            Gizmos.DrawLine(m_Nodes[i].transform.position + new Vector3(0, 0.5f, 0), m_Nodes[i + 1].transform.position + new Vector3(0, 0.5f, 0));
        }
        Gizmos.DrawLine(m_Nodes[0].transform.position + new Vector3(0, 0.5f, 0), m_Nodes[m_Nodes.Length - 1].transform.position + new Vector3(0, 0.5f, 0));
    }
#endif
}

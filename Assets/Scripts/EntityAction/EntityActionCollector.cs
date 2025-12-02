using System.Collections.Generic;
using UnityEngine;

public class EntityActionCollector : MonoBehaviour
{
    [SerializeField] private Transform m_ParentTransformWithActions;

    private List<EntityAction> m_AllAction = new List<EntityAction>();

    private void Awake()
    {
        for (int i = 0; i < m_ParentTransformWithActions.childCount; i++)
        {
            if (m_ParentTransformWithActions.GetChild(i).gameObject.activeSelf == true)
            {
                EntityAction action = m_ParentTransformWithActions.GetChild(i).GetComponent<EntityAction>();
                if (action != null)
                {
                    m_AllAction.Add(action);
                }
            }
        }
    }
    public T GetAction<T>() where T : EntityAction
    {
        for (int i = 0; i < m_AllAction.Count; i++)
        {
            if (m_AllAction[i] is T) return (T)m_AllAction[i];
        }
        return null;
    }
    public List<T> GetActionList<T>() where T : EntityAction
    {
        List<T> actions = new List<T>();
        for (int i = 0; i < m_AllAction.Count; i++)
        {
            if (m_AllAction[i] is T) actions.Add((T)m_AllAction[i]);
        }
        return actions;
    }
}

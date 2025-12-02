using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    public UnityAction Completed;

    [SerializeField] private Quest m_NextQuest;
    [SerializeField] private QuestProperties m_Properties;
    [SerializeField] private Transform m_ReachedPoint;

    public Quest NextQuest => m_NextQuest;
    public QuestProperties Properties => m_Properties;
    public Transform ReachedPoint => m_ReachedPoint;


    protected virtual void Update()
    {
        UpdateCompleteCondition();
    }
    protected virtual void UpdateCompleteCondition() { }
}

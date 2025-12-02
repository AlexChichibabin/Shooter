using UnityEngine;
using UnityEngine.Events;

public class QuestCollector : MonoBehaviour
{
    public UnityAction<Quest> QuestReceived;
    public UnityAction<Quest> QuestCompleted;
    public UnityAction LastQuestCompleted;

    [SerializeField] private Quest m_CurrentQuest;
    public Quest CurrentQuest => m_CurrentQuest;


    private void Start()
    {
        //if(m_CurrentQuest != null) AssignQuest(m_CurrentQuest);
    }
    public void AssignQuest(Quest quest)
    {
        m_CurrentQuest = quest;

        if (QuestReceived != null)
        {
            QuestReceived.Invoke(m_CurrentQuest);
            m_CurrentQuest.Completed += OnCompleted;
        } 
    }
    private void OnDestroy()
    {
        if(m_CurrentQuest != null) m_CurrentQuest.Completed -= OnCompleted;
    }
    private void Update()
    {
        if (m_CurrentQuest == null) return;
        if (m_CurrentQuest.Completed == null) m_CurrentQuest.Completed += OnCompleted;
    }

    private void OnCompleted()
    {
        m_CurrentQuest.Completed -= OnCompleted;

        if(QuestCompleted != null) QuestCompleted.Invoke(m_CurrentQuest);

        if (m_CurrentQuest.NextQuest != null)
        {
            AssignQuest(m_CurrentQuest.NextQuest);
        }
        else
        {
            if (LastQuestCompleted != null) LastQuestCompleted.Invoke();
        }
    }
}

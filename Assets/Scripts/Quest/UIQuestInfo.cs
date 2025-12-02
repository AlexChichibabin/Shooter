using TMPro;
using UnityEngine;

public class UIQuestInfo : MonoBehaviour
{
    [SerializeField] private QuestCollector m_QuestCollector;
    [SerializeField] private TextMeshProUGUI m_Description;
    [SerializeField] private TextMeshProUGUI m_Task;


    private void Start()
    {
        m_Description.gameObject.SetActive(false);
        m_Task.gameObject.SetActive(false);
        if (m_QuestCollector.QuestReceived == null) m_QuestCollector.QuestReceived += OnQuestReceived;
        if (m_QuestCollector.QuestCompleted == null) m_QuestCollector.QuestCompleted += OnQuestCompleted;
    }
    private void OnDestroy()
    {
        m_QuestCollector.QuestReceived -= OnQuestReceived;
        m_QuestCollector.QuestCompleted -= OnQuestCompleted;
    }
    private void Update()
    {
        if (m_QuestCollector.CurrentQuest == null) return;
        if (m_Description.text != m_QuestCollector.CurrentQuest.Properties.Description ||
            m_Task.text != m_QuestCollector.CurrentQuest.Properties.Task)
        {
            AssignQuestInfo(m_QuestCollector.CurrentQuest);
        }
    }
    public void DescribeQuestCollector()
    {
        if(m_QuestCollector.QuestReceived == null) m_QuestCollector.QuestReceived += OnQuestReceived;
        if (m_QuestCollector.QuestCompleted == null) m_QuestCollector.QuestCompleted += OnQuestCompleted;
    }
    private void OnQuestReceived(Quest quest)
    {
        AssignQuestInfo(quest);
    }
    private void OnQuestCompleted(Quest quest)
    {
        m_Description.gameObject.SetActive(false);
        m_Task.gameObject.SetActive(false);
    }
    private void AssignQuestInfo(Quest quest)
    {
        m_Description.text = quest.Properties.Description;
        m_Task.text = quest.Properties.Task;
        m_Description.gameObject.SetActive(true);
        m_Task.gameObject.SetActive(true);
    }
}

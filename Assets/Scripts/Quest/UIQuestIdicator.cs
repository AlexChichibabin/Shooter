using UnityEngine;
using UnityEngine.UI;

public class UIQuestIdicator : MonoBehaviour
{
    [SerializeField] private QuestCollector m_QuestCollector;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private Image m_Indicator;

    private Transform ReachedPoint;

    private void Start()
    {
        if (m_QuestCollector.QuestReceived == null) m_QuestCollector.QuestReceived -= OnQuestReceived;
        if (m_QuestCollector.QuestCompleted == null) m_QuestCollector.QuestCompleted -= OnQuestCompleted;
        m_Indicator.gameObject.SetActive(m_QuestCollector.CurrentQuest != null);
    }
    private void OnDestroy()
    {
        m_QuestCollector.QuestReceived -= OnQuestReceived;
        m_QuestCollector.QuestCompleted -= OnQuestCompleted;
    }
    private void Update()
    {
        if (m_QuestCollector != null) m_QuestCollector.QuestReceived += OnQuestReceived;
        if (m_QuestCollector != null) m_QuestCollector.QuestCompleted += OnQuestCompleted;

        if (ReachedPoint == null)
        {
            if (m_QuestCollector && m_QuestCollector.CurrentQuest && m_QuestCollector.CurrentQuest.ReachedPoint)
                ReachedPoint = m_QuestCollector.CurrentQuest.ReachedPoint;
            else return;
        }
            

        Vector3 pos = m_Camera.WorldToScreenPoint(ReachedPoint.position);

        if (pos.z > 0)
        {
            if (pos.x < 0) pos.x = 0;
            if (pos.x > Screen.width) pos.x = Screen.width;
        }
        else
        {
            Vector3 camViewYDir = m_Camera.transform.forward;
            Vector3 camViewRightDir = m_Camera.transform.right;
            Vector3 camToPointYDir = ReachedPoint.position - m_Camera.transform.position;

            camViewYDir.y = 0;
            camToPointYDir.y = 0;
            camViewRightDir.y = 0;

            float angle = Vector3.Angle(camViewYDir, camToPointYDir);
            float angleByRight = Vector3.Angle(camViewRightDir, camToPointYDir);

            if (angleByRight < 90)
            pos.x = Screen.width;
            else pos.x = 0;
        }
        if (pos.y < 0) pos.y = 0;
        if (pos.y > Screen.height) pos.y = Screen.height;

        m_Indicator.transform.position = pos;
    }

    private void OnQuestReceived(Quest quest)
    {
        m_Indicator.gameObject.SetActive(true);
        ReachedPoint = quest.ReachedPoint;
    }

    private void OnQuestCompleted(Quest quest)
    {
        m_Indicator.gameObject.SetActive(false);
    }
}

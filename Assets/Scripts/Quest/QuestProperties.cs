using UnityEngine;

[CreateAssetMenu(fileName = "QuestProperties", menuName = "Scriptable Objects/QuestProperties")]
public class QuestProperties : ScriptableObject
{
    [TextArea]
    [SerializeField] private string m_Description;
    public string Description => m_Description;

    [TextArea]
    [SerializeField] private string m_Task;
    public string Task => m_Task;
}

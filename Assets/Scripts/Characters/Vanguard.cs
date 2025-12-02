using UnityEngine;

public class Vanguard : Destructible
{
    [SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private float m_DamageFall;

    protected override void Start()
    {
        base.Start();

        m_CharacterMovement.Land += OnLand;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        m_CharacterMovement.Land -= OnLand;
    }
    private void OnLand(Vector3 velocity)
    {
        if(Mathf.Abs( velocity.y) < 10) return;

        ApplyDamage((int)(Mathf.Abs(velocity.y) * m_DamageFall), this);
    }

    protected override void OnDeath()
    {
        m_CharacterMovement.Land -= OnLand;

        EventOnDeath?.Invoke();
    }

    #region Serialize

    [System.Serializable]
    public class AIPlayerState
    {
        public Vector3 position;
        public Quaternion rotation;
        public int hitPoints;
        public Quest currentQuest;
        public AIPlayerState(Vector3 pos, Quaternion rot, int hits, Quest quest)
        {
            position = pos;
            rotation = rot;
            hitPoints = hits;
            this.currentQuest = quest;
        }
    }
    public override string SerializeState()
    {
        AIPlayerState s = new AIPlayerState(transform.position, transform.rotation, CurrentHitPoints, GetComponent<QuestCollector>()?.CurrentQuest);
        //if (GetComponent<QuestCollector>() != null) s.CurrentQuest = GetComponent<QuestCollector>().CurrentQuest;
        return JsonUtility.ToJson(s);
    }

    public override void DeserializeState(string state)
    {
        AIPlayerState s = JsonUtility.FromJson<AIPlayerState>(state);

        transform.position = s.position;
        transform.rotation = s.rotation;
        SetHitPoints(s.hitPoints);

        QuestCollector collector = GetComponent<QuestCollector>();
        if (collector != null)
        {
            //GetComponentInChildren<UIQuestInfo>()?.DescribeQuestCollector();

            collector.AssignQuest(s.currentQuest);
        }
        
    }
    #endregion
}

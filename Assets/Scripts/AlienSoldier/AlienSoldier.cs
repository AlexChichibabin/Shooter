using UnityEngine;

public class AlienSoldier : Destructible, ISoundListener
{
    [SerializeField] private Weapon m_Weapon;
    [SerializeField] private SpreadShootRig m_SpreadShootRig;
    [SerializeField] private AIAlienSoldier m_AIAlienSoldier; 
    [SerializeField] private float m_HearingDistance;
    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }
    public void LookAt(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 400);
    }
    public void Fire(Vector3 target)
    {
        if(m_Weapon.CanFire == false) return;

        m_Weapon.FirePointLookAt(target);
        m_Weapon.Fire();
        //m_SpreadShootRig.Spread(true);
    }
    public void Heard(float distance)
    {
        if (distance <= m_HearingDistance)
        {
            m_AIAlienSoldier.OnHeard();
        }
    }

    #region Serialize

    [System.Serializable]
    public class AIAlienState
    {
        public Vector3 position;
        public Quaternion rotation;
        public int hitPoints;
        public int behaviour;
        public AIAlienState(Vector3 pos, Quaternion rot, int hits, int behav)
        {
            position = pos;
            rotation = rot;
            hitPoints = hits;
            behaviour = behav;
        }
    }
    public override string SerializeState()
    {
        AIAlienState s = new AIAlienState(transform.position, transform.rotation, CurrentHitPoints, (int)m_AIAlienSoldier.Behaviour);
        //if (GetComponent<QuestCollector>() != null) s.currentQuest = GetComponent<QuestCollector>().CurrentQuest;
        return JsonUtility.ToJson(s);
    }

    public override void DeserializeState(string state)
    {
        AIAlienState s = JsonUtility.FromJson<AIAlienState>(state);

        m_AIAlienSoldier.SetPosition(s.position);
        transform.rotation = s.rotation;
        SetHitPoints(s.hitPoints);
        m_AIAlienSoldier.Behaviour = (AIAlienSoldier.AIBehaviour) s.behaviour;
    }
    #endregion
}

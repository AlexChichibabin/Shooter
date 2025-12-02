using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// some destructible object that can own hitpoints
/// </summary>
public class Destructible : Entity, ISerializableEntity
{
    #region Properties

    public const int TeamIdNeutral = 0;

    [SerializeField] private bool m_Indestructible;
    [SerializeField] private int m_HitPointStat;
    [SerializeField] private int m_HitPointRegenPerSecond;
    [SerializeField] private int m_ScoreValue;
    [SerializeField] private UnityEvent m_EventOnDeath;
    [SerializeField] private UnityEvent m_EventOnGetDamage;
    [SerializeField] private int m_TeamId;

    private int m_CurrentHitPoints;
    private Vector3 m_LastPosition;
    protected bool m_IsDeath = false;
    private bool m_IndestructibleTimeIsOver = false;
    private float additionalHP = 0.0f;
    private static HashSet<Destructible> m_AllDestructibles;

    public UnityAction<Destructible> OnGetDamage;
    public bool IsIndestructible => m_Indestructible;
    public int HitPoints => m_HitPointStat;
    public int CurrentHitPoints => m_CurrentHitPoints;
    public Vector3 LastPosition => m_LastPosition;
    public bool IsDeath => m_IsDeath;
    public bool IndestructibleTimeIsOver => m_IndestructibleTimeIsOver;
    public int ScoreValue => m_ScoreValue;
    public UnityEvent EventOnDeath => m_EventOnDeath;
    public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;
    public int TeamId => m_TeamId;


    #endregion


    #region Unity Events

    protected virtual void Start()
    {
        m_CurrentHitPoints = m_HitPointStat;
    }
    private void Update()
    {
        UpdateHealthRegen();
    }
    protected virtual void OnEnable()
    {
        if (m_AllDestructibles == null)
            m_AllDestructibles = new HashSet<Destructible>();

        m_AllDestructibles.Add(this);
    }
    protected virtual void OnDestroy()
    {
        if (this != null && m_AllDestructibles != null) m_AllDestructibles.Remove(this);
    }

    #endregion


    #region Public API

    public static Destructible FindNearest(Vector3 position)
    {
        float minDist = float.MaxValue;
        Destructible target = null;

        foreach (Destructible dest in m_AllDestructibles)
        {
            float curDist = Vector3.Distance(dest.transform.position, position);
            if (curDist < minDist)
            {
                minDist = curDist;
                target = dest;
            }
        }
        return target;
    }
    public static Destructible FindNearestNonTeamMember(Destructible destructible)
    {
        float minDist = float.MaxValue;
        Destructible target = null;

        foreach (Destructible dest in m_AllDestructibles)
        {
            float curDist = Vector3.Distance(dest.transform.position, destructible.transform.position);
            if (curDist < minDist && dest.TeamId != destructible.TeamId)
            {
                minDist = curDist;
                target = dest;
            }
        }
        return target;
    }
    public static List<Destructible> GetAllTeamMembers(int teamId)
    {
        List<Destructible> teamDestructibles = new List<Destructible>();

        foreach (Destructible dest in m_AllDestructibles)
        {
            if(teamId == dest.TeamId)
                teamDestructibles.Add(dest);
        }
        return teamDestructibles;
    }
    public static List<Destructible> GetAllNonTeamMembers(int teamId)
    {
        List<Destructible> teamDestructibles = new List<Destructible>();

        foreach (Destructible dest in m_AllDestructibles)
        {
            if (teamId != dest.TeamId)
                teamDestructibles.Add(dest);
        }
        return teamDestructibles;
    }
    public void HealFull() => m_CurrentHitPoints = m_HitPointStat;
    public void SetHitPoints(int hitPoints) => m_CurrentHitPoints = Mathf.Clamp(hitPoints, 0, m_HitPointStat);
    public void ApplyDamage(int damage, Destructible other)
    {
        if (m_Indestructible || m_IsDeath == true) return;

        m_CurrentHitPoints -= damage;

        m_EventOnGetDamage?.Invoke();
        OnGetDamage?.Invoke(other);

        if (m_CurrentHitPoints <= 0)
        {
            m_IsDeath = true;
            OnDeath();
        }
    }
    public void KillSelf()
    {
        m_CurrentHitPoints = 0;

        m_EventOnGetDamage?.Invoke();

        m_IsDeath = true;
        OnDeath();
    }
    public void ApplyHeal(int health)
    {
        m_CurrentHitPoints += health;
        if (m_CurrentHitPoints > m_HitPointStat) m_CurrentHitPoints = m_HitPointStat;
    }
    public void SetIndestructibility(bool indestructible)
    {
        m_Indestructible = indestructible;
        m_IndestructibleTimeIsOver = indestructible;
    }

    #endregion


    protected virtual void OnDeath()
    {
        //m_LastPosition = gameObject.transform.position;

        Destroy(gameObject);

        m_EventOnDeath?.Invoke();
    }

    private void UpdateHealthRegen()
    {
        additionalHP += (float)m_HitPointRegenPerSecond * Time.deltaTime;
        if (additionalHP > 1)
        {
            m_CurrentHitPoints = Mathf.Clamp((m_CurrentHitPoints + (int)additionalHP), 0, m_HitPointStat);
            additionalHP -= (int)additionalHP;
        }
    }

    #region Serialize
    [System.Serializable]
    public class State
    {
        public Vector3 position;
        public Quaternion rotation;
        public int hitPoints;
        public State(Vector3 pos, Quaternion rot, int hits)
        {
            position = pos;
            rotation = rot;
            hitPoints = hits;
        }
    }

    [SerializeField] private long m_EntityID;
    public long EntityId => m_EntityID;
    public virtual bool IsSerializable()
    {
        return m_CurrentHitPoints > 0;
    }

    public virtual string SerializeState()
    {
        State s = new State(transform.position, transform.rotation, m_CurrentHitPoints);
        return JsonUtility.ToJson(s);
    }

    public virtual void DeserializeState(string state)
    {
        State s = JsonUtility.FromJson<State>(state);

        transform.position = s.position;
        transform.rotation = s.rotation;
        m_CurrentHitPoints = s.hitPoints;
    }
    #endregion
}


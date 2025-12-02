using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAlienSoldier : MonoBehaviour
{
    public enum AIBehaviour
    { 
        Null,
        Idle,
        PatrolRandom,
        PatrolCircle,
        PursuitTarget,
        SeekTarget
    }

    [SerializeField] private AIBehaviour m_Behaviour;
    [SerializeField] private AlienSoldier m_AlienSoldier;
    [SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private NavMeshAgent m_Agent;
    [SerializeField] private ColliderViewer m_ColliderViewer;
    [SerializeField] private int m_PatrolPathNodeIndex;
    [SerializeField] private float m_AimingDistance;
    [SerializeField] private float m_PatrolStopDistance;


    [Header("Seek")]
    [SerializeField] private float m_SeekRadius;
    [SerializeField] private float m_SeekDuration;
    [SerializeField] private SeekArea seekArea;


    private PatrolPath m_Path;
    private NavMeshPath m_NavMeshPath;
    private PatrolPathNode m_CurrentPathNode;
    private GameObject potentialTarget;
    private Transform pursuitTarget;
    private Vector3 seekTarget;
    private Timer seekTimer;
    //private Destructible listeningTarget;
    //private Noiser listeningNoiseTarget;
    private bool isPlayerDetected;
    private float defaultStopDistance;

    public bool IsPursuing => pursuitTarget != null;
    public bool IsSeeking => seekTarget != null;
    public AIBehaviour Behaviour { get { return m_Behaviour; } set { m_Behaviour = value; } }


    IEnumerator m_SetBehavOnTimeCoroutine;

    // UnityEvent
    private void Awake()
    {
        //defaultStopDistance = m_Agent.stoppingDistance;
        /*enabled = false; Это и коррутина нужны были чтобы при загрузке из сохраненния аент понял его положение. Если через простое изменение трансформы, то не воспринимает корректно
        StartCoroutine(ComponentsStartOrderNumerator());*/
    }
    private void Start() // Можно делать UnAiming при смерти. Подписка на EventOnDeath AlienSoldier
    {
        FindPatrolPath();
        m_CharacterMovement.UpdatePosition = false;
        m_NavMeshPath = new NavMeshPath();

        StartBehaviour(m_Behaviour);
        m_AlienSoldier.OnGetDamage += OnGetDamage;
        m_AlienSoldier.EventOnDeath.AddListener(OnDeath);

        
    }
    /*IEnumerator ComponentsStartOrderNumerator()
    {
        yield return new WaitForEndOfFrame();
        m_Agent.enabled = true;
        yield return new WaitForEndOfFrame();
        enabled = true;
    }*/

    private void OnDeath()
    {
        if (isPlayerDetected == true) SendPlayerStopPursuit();
    }

    private void OnDestroy()
    {
        m_AlienSoldier.OnGetDamage -= OnGetDamage;
        m_AlienSoldier.EventOnDeath.RemoveListener(OnDeath);
        if (seekTimer != null)
            seekTimer.OnTimeRunOut -= OnTimerRunOut;
    }
    private void Update()
    {
        SyncAgentAndCharacterMovement();

        UpdateAI();
    }
    
    // Handler
    private void OnGetDamage(Destructible other)
    {
        ActionAssignTargetAllTeamMember(other.transform);
    }

    // Listening
    public void OnHeard()
    {
        SendPlayerStartPursuit();
        pursuitTarget = potentialTarget.transform;
        ActionAssignTargetAllTeamMember(pursuitTarget);
    }

    // Public API
    public void SetPosition(Vector3 position) => m_Agent.Warp(position);
    public void SetPursueTarget(Transform target) => pursuitTarget = target;

    // AI
    private void UpdateAI()
    {
        ActionUpdateTarget();

        if (m_Behaviour == AIBehaviour.Idle) return;
        
        if (m_Behaviour == AIBehaviour.PatrolRandom)
        {
            if (AgentReachedDestination() == true)
            {
                m_SetBehavOnTimeCoroutine = SetBehaviourOnTime(AIBehaviour.Idle, m_CurrentPathNode.IdleTime);
                StartCoroutine(m_SetBehavOnTimeCoroutine);
            }
        }
        if (m_Behaviour == AIBehaviour.PatrolCircle)
        {
            if (AgentReachedDestination() == true)
            {
                m_SetBehavOnTimeCoroutine = SetBehaviourOnTime(AIBehaviour.Idle, m_CurrentPathNode.IdleTime);
                StartCoroutine(m_SetBehavOnTimeCoroutine);
            }
        }
        if (m_Behaviour == AIBehaviour.PursuitTarget)
        {
            m_Agent.CalculatePath(pursuitTarget.position, m_NavMeshPath);
            m_Agent.SetPath(m_NavMeshPath);

            if (Vector3.Distance(transform.position, pursuitTarget.position) <= m_AimingDistance)
            {
                m_CharacterMovement.Aiming();
                Debug.Log("m_CharacterMovement.Aiming()");
                m_AlienSoldier.LookAt(pursuitTarget.position);
                m_AlienSoldier.Fire(pursuitTarget.position + new Vector3(0, 1, 0));
            }
            else
            {
                m_Agent.isStopped = false;
                Debug.Log("No");
                m_CharacterMovement.UnAiming();
            }
                
        }
        if (m_Behaviour == AIBehaviour.SeekTarget)
        {
            m_Agent.CalculatePath(seekTarget, m_NavMeshPath);
            m_Agent.SetPath(m_NavMeshPath);

            if (AgentReachedDestination() == true)
            {
                if(seekArea!= null) seekTarget = seekArea.GetRandomInsideZone();
                else if(seekTimer == null || seekTimer.isActiveAndEnabled == false || seekTimer.IsCompleted == true)
                    StartBehaviour(AIBehaviour.PatrolRandom);
            }
        }
    }
    // Actions
    private void FindPotentialTarget()
    {
        if(Player.Instance != null) potentialTarget = Player.Instance.gameObject;
    }
    private void ActionUpdateTarget()
    {
        if (potentialTarget == null)
        {
            FindPotentialTarget();

            if (potentialTarget == null) return;
        }
        FindSeekArea();

        if (m_ColliderViewer.IsObjectVisible(potentialTarget) == true)
        {
            SendPlayerStartPursuit();
            pursuitTarget = potentialTarget.transform;
            ActionAssignTargetAllTeamMember(pursuitTarget);
        }
        else
        {
            if (seekTimer != null && seekTimer.isActiveAndEnabled == true) return;
            if (pursuitTarget != null)
            {
                seekTarget = pursuitTarget.position;
                if (seekArea != null)
                {
                    seekArea.transform.position = seekTarget;
                }
                pursuitTarget = null;
                StartBehaviour(AIBehaviour.SeekTarget);
            }
        }
    }

    // Behaviour
    private void StartBehaviour(AIBehaviour state)
    {
        if(m_AlienSoldier.IsDeath == true) return;
        if (state != AIBehaviour.SeekTarget)
        {
            m_Agent.stoppingDistance = defaultStopDistance;
            if (seekTimer != null) seekTimer.enabled = false;
        }
        if (state == AIBehaviour.PursuitTarget || state == AIBehaviour.SeekTarget)
        {
            if(m_SetBehavOnTimeCoroutine != null)
                StopCoroutine(m_SetBehavOnTimeCoroutine); // Maybe
        }  
        if (state == AIBehaviour.Idle)
        {
            //m_Agent.isStopped = true;
            m_CharacterMovement.UnAiming();
        }
        if (state == AIBehaviour.PatrolRandom)
        {
            m_Agent.isStopped = false;
            m_Agent.stoppingDistance = m_PatrolStopDistance;
            m_CharacterMovement.UnAiming();
            SetDestinationByPathNode(m_Path.GetRandomPathNode());
            if (isPlayerDetected == true) SendPlayerStopPursuit();
        }
        if (state == AIBehaviour.PatrolCircle)
        {
            m_Agent.isStopped = false;
            m_Agent.stoppingDistance = m_PatrolStopDistance;
            m_CharacterMovement.UnAiming();
            SetDestinationByPathNode(m_Path.GetNextPathNode(ref m_PatrolPathNodeIndex));
            if (isPlayerDetected == true) SendPlayerStopPursuit();
        }
        if (state == AIBehaviour.PursuitTarget)
        {
            m_Agent.isStopped = false;
            m_Agent.stoppingDistance = m_AimingDistance * 0.75f;
            m_CharacterMovement.Aiming();
        }
        if (state == AIBehaviour.SeekTarget)
        {
            m_Agent.isStopped = false;
            m_Agent.stoppingDistance = m_PatrolStopDistance;
            m_CharacterMovement.UnAiming();
            if (seekTimer == null)
            {
                seekTimer = Timer.CreateTimer(m_SeekDuration);
                seekTimer.OnTimeRunOut += OnTimerRunOut;
                seekTimer.enabled = true;
            }
            else
            {
                seekTimer.enabled = true;
                seekTimer.Restart();
            }
        }

        m_Behaviour = state;
    }
    private void OnTimerRunOut()
    {
        StartBehaviour(AIBehaviour.PatrolRandom);
        seekTimer.enabled = false;
    }
    IEnumerator SetBehaviourOnTime(AIBehaviour state, float second)
    {
        AIBehaviour previous = m_Behaviour;
        m_Behaviour = state;
        StartBehaviour(m_Behaviour);

        yield return new WaitForSeconds(second);

        StartBehaviour(previous);
    }
    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMembers(m_AlienSoldier.TeamId);

        foreach (Destructible dest in team)
        {
            AIAlienSoldier ai = dest.transform.root.GetComponent<AIAlienSoldier>();

            if (ai != null && ai.enabled == true)
            {
                ai.SetPursueTarget(other);
                ai.StartBehaviour(AIBehaviour.PursuitTarget);
            }
        }
    }

    // Private methods
    private void FindPatrolPath()
    {
        if (m_Path == null)
        {
            PatrolPath[] patrolPaths = FindObjectsByType<PatrolPath>(FindObjectsSortMode.None);
            float minDist = float.MaxValue;

            for (int i = 0; i < patrolPaths.Length; i++)
            {
                if (Vector3.Distance(transform.position, patrolPaths[i].transform.position) < minDist)
                {
                    m_Path = patrolPaths[i];
                    minDist = Vector3.Distance(transform.position, m_Path.transform.position);
                }
            }
        }
    }
    private void SetDestinationByPathNode(PatrolPathNode node)
    {
        m_CurrentPathNode = node;
        m_Agent.CalculatePath(m_CurrentPathNode.transform.position, m_NavMeshPath);
        m_Agent.SetPath(m_NavMeshPath);
    }
    private bool AgentReachedDestination()
    {
        if (m_Agent.pathPending == false)
        {
            if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
            {
                if (m_Agent.hasPath == false || m_Agent.velocity.sqrMagnitude == 0.0f)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }
    private void SyncAgentAndCharacterMovement()
    {
        m_Agent.speed = m_CharacterMovement.CurrentSpeed;

        float factor = m_Agent.velocity.magnitude / m_Agent.speed;
        m_CharacterMovement.TargetDirectionControl = transform.InverseTransformDirection(m_Agent.velocity.normalized) * factor;
    }
    private void SendPlayerStartPursuit()
    {
        if (isPlayerDetected == false)
        {
            Player.Instance.StartPursuit();
            isPlayerDetected = true;
        }
    }
    private void SendPlayerStopPursuit()
    {
        if (isPlayerDetected == true)
        {
            Player.Instance.StopPursuit();
            isPlayerDetected = false;
        } 
    }
    private void FindSeekArea()
    {
        if (seekArea == null) seekArea = FindAnyObjectByType<SeekArea>();
    }
}

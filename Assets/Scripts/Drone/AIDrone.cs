using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Drone))]
public class AIDrone : MonoBehaviour
{
    [SerializeField] private float m_DistanceDeltaToChangeMovementTarget;
    [SerializeField] private ColliderViewer m_ColliderViewer;

    private Drone m_Drone;
    private Vector3 m_MovementTarget;
    private Transform m_ShootTarget;
    private CubeArea m_MovementArea;


    //UnityEvents
    private void Start()
    {
        m_Drone = GetComponent<Drone>();
        m_Drone.EventOnDeath.AddListener(OnDroneDeath);
        m_Drone.OnGetDamage += OnGetDamage;
        FindMovementArea();
    }

    private void Update()
    {
        if(m_MovementArea == null) FindMovementArea();
        UpdateAI();
    }
    private void OnDestroy()
    {
        m_Drone?.EventOnDeath?.RemoveListener(OnDroneDeath);
        if(m_Drone != null) m_Drone.OnGetDamage -= OnGetDamage;
    }
    //Handlers
    private void OnDroneDeath()
    {
        enabled = false;
    }
    private void OnGetDamage(Destructible other)
    {
        ActionAssignTargetAllTeamMember(other.transform);
    }

    //AI
    private void UpdateAI()
    {
        ActionFindShootingTarget();

        ActionMove();

        ActionFire();
    }

    //Action
    private void ActionFindShootingTarget()
    {
        Transform potentialTarget = FindShootTarget();

        if (potentialTarget != null)
        {
            ActionAssignTargetAllTeamMember(potentialTarget);
        }
    }
    private void ActionMove()
    {
        if (m_MovementArea == null) return;

        if (Vector3.Distance(transform.position, m_MovementTarget) < m_DistanceDeltaToChangeMovementTarget)
            m_MovementTarget = m_MovementArea.GetRandomInsideZone();
        if (Physics.Linecast(transform.position, m_MovementTarget) == true)
            m_MovementTarget = m_MovementArea.GetRandomInsideZone();

        m_Drone.MoveTo(m_MovementTarget);

        if (m_ShootTarget != null)
            m_Drone.LookAt(m_ShootTarget.position);
        else
            m_Drone.LookAt(m_MovementTarget);
    }
    private void ActionFire()
    {
        if (m_ShootTarget != null && m_ColliderViewer.IsObjectVisible(m_ShootTarget.gameObject) == true)
            m_Drone.Fire(m_ShootTarget.position);
    }
    //Methods
    private void FindMovementArea()
    {
        if (m_MovementArea == null)
        {
            CubeArea[] cubeAreas = FindObjectsByType<CubeArea>(FindObjectsSortMode.None);
            float minDist = float.MaxValue;

            for (int i = 0; i < cubeAreas.Length; i++)
            {
                if (Vector3.Distance(transform.position, cubeAreas[i].transform.position) < minDist)
                {
                    m_MovementArea = cubeAreas[i];
                    minDist = Vector3.Distance(transform.position, m_MovementArea.transform.position);
                }
            }
        }
    }
    public void SetShootTarget(Transform target)
    {
        m_ShootTarget = target;
    }
    private Transform FindShootTarget()
    {
        List<Destructible> targets = Destructible.GetAllNonTeamMembers(m_Drone.TeamId);
        for (int i = 0; i < targets.Count; i++)
        {
            //Debug.Log(m_ColliderViewer.IsObjectVisible(targets[i].gameObject));

            if(m_ColliderViewer.IsObjectVisible(targets[i].gameObject) == true)
                return targets[i].transform;
        }
        return null;
    }
    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMembers(m_Drone.TeamId);

        foreach (Destructible dest in team)
        {
            AIDrone ai = dest.transform.root.GetComponent<AIDrone>();

            if (ai != null && ai.enabled == true)
            {
                ai.SetShootTarget(other);
            }
        }
    }
}

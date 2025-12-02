using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestKillDestructibles : Quest
{
    [SerializeField] private Destructible[] m_Destructibles;
    private long[] m_DestructiblesId;
    private int destrutiblesTeamId;
    private int AmountDestructibleDead = 0;

    private void Start()
    {
        if (m_Destructibles == null) return;

        m_DestructiblesId = new long[m_Destructibles.Length];
        destrutiblesTeamId = m_Destructibles[0].TeamId;

        for (int i = 0; i < m_Destructibles.Length; i++)
        {
            m_Destructibles[i].EventOnDeath.AddListener(OnDestructibleDead);
            m_DestructiblesId[i] = m_Destructibles[i].EntityId;
        }
    }
    protected override void Update()
    {
        base.Update();

        if (m_Destructibles[0] == null)
        {
            for (int i = 0; i < m_Destructibles.Length; i++)
            {
                if (m_Destructibles[i] == null)
                {
                    AssignDestructibles();
                    break;
                }
            }
        }
    }
    private void AssignDestructibles()
    {
        List<Destructible> destrs = Destructible.GetAllTeamMembers(destrutiblesTeamId);
        if (destrs != null && destrs.Count < m_Destructibles.Length) Array.Resize(ref m_Destructibles, destrs.Count);
        else Completed?.Invoke();
        for (int i = 0; i < m_Destructibles.Length; i++)
        {
            for (int j = 0; j < destrs.Count; j++)
            {
                if (m_Destructibles[i].EntityId == destrs[j].EntityId)
                {
                    m_Destructibles[i] = destrs[j];
                    m_Destructibles[i].EventOnDeath.AddListener(OnDestructibleDead);
                }   
            }
        }
    }

    private void OnDestructibleDead()
    {
        AmountDestructibleDead++;

        if (AmountDestructibleDead == m_Destructibles.Length)
        {
            for (int i = 0; i < m_Destructibles.Length; i++)
            {
                m_Destructibles[i].EventOnDeath.RemoveListener(OnDestructibleDead);
            }

            if (Completed != null)
            {
                Completed.Invoke();
            }
        }
    }
}

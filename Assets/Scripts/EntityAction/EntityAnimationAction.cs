using UnityEngine;

public class EntityAnimationAction : EntityAction
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private string m_ActionAnimationName;
    [SerializeField] private float m_StartDuration;

    private Timer m_Timer;
    private bool m_IsPlayingAnimation;

    public override void StartAction()
    {
        base.StartAction();
        m_Animator.CrossFade(m_ActionAnimationName, m_StartDuration);
        m_Timer = Timer.CreateTimer(m_StartDuration, true);
        m_Timer.OnTick += OnTimerTick;
    }
    private void OnDestroy()
    {
        if(m_Timer != null) m_Timer.OnTick -= OnTimerTick;
    }
    public override void EndAction()
    {
        base.EndAction();
        m_Timer.OnTick -= OnTimerTick;
        m_Animator.transform.rotation = transform.root.rotation; // HARDCODE?
        m_Animator.transform.position = transform.root.position; // HARDCODE?
    }
    private void OnTimerTick()
    {
        if (m_Animator == null)
        {
            m_Timer.OnTick -= OnTimerTick;
            return;
        }
            
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName(m_ActionAnimationName) == true)
        {
            m_IsPlayingAnimation = true;
        }
        if (m_IsPlayingAnimation == true && m_Animator.GetCurrentAnimatorStateInfo(0).IsName(m_ActionAnimationName) == false)
        {
            m_IsPlayingAnimation = false;
            EndAction();
        }
    }
}

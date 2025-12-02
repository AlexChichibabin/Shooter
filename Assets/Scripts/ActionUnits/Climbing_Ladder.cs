using System.Collections;
using UnityEngine;

public class Climbing_Ladder : TriggerInteraction
{
    [SerializeField] private Transform m_LadderTop;
    [SerializeField] private Transform m_LadderStand;
    [SerializeField] private float m_ClimbSpeedRate;
    private Vector3 m_TopPosition => new Vector3(m_ActionProperties.InteractTransform.position.x, m_LadderTop.position.y, m_ActionProperties.InteractTransform.position.z);
    private Vector3 m_FinalPosition => new Vector3(m_LadderStand.position.x, m_LadderStand.position.y, m_LadderStand.position.z);


    protected override void OnStartAction(GameObject owner)
    {
        base.OnStartAction(owner);

        Destructible des = owner.transform.root.GetComponent<Destructible>();

        if (des != null) StartCoroutine(ClimbNumerator(owner, des));
        
    }

    IEnumerator ClimbNumerator(GameObject owner, Destructible des)
    {
        des.GetComponent<CharacterMovement>()?.SetClimbing(true);

        while (Vector3.Distance(owner.transform.position, m_LadderTop.position) > 0.1)
        {
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, m_TopPosition, Time.deltaTime * m_ClimbSpeedRate);
            yield return new WaitForEndOfFrame();
        }
        while (Vector3.Distance(owner.transform.position, m_LadderStand.position) > 0.1)
        {
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, m_FinalPosition, Time.deltaTime * m_ClimbSpeedRate);
            yield return new WaitForEndOfFrame();
        }
        Animator anim = des.transform.GetComponentInChildren<Animator>();
        anim.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        anim.gameObject.SetActive(true);
        m_Action.IsCanEnd = true;
        m_Action.EventOnEnd?.Invoke();
        StopCoroutine(ClimbNumerator(owner, des));
    }
}

using System.Collections;
using UnityEngine;

public class ActionPickUp : ActionInteract
{
    [SerializeField] private float m_MoveToInteractTransformSpeedRate;
    public override void StartAction()
    {
        if (IsCanStart == false) return;
        base.StartAction();
        
        if(m_Owner != null) StartCoroutine(MoveToInteractTransformNumerator());
    }

    IEnumerator MoveToInteractTransformNumerator()
    {
        while (Vector3.Distance(m_Owner.position, Properties.InteractTransform.position) > 0.1f)
        {
            m_Owner.position = Vector3.MoveTowards(m_Owner.position, Properties.InteractTransform.position, Time.deltaTime * m_MoveToInteractTransformSpeedRate);
            LookAt(Properties.InteractTransform.root.position);
            yield return new WaitForEndOfFrame();
        }
        IsCanEnd = true;
    }
    private void LookAt(Vector3 target)
    {
        Vector3 targetDir = m_Owner.position - target;
        Quaternion targetRotation = Quaternion.LookRotation(-targetDir, Vector3.up);
        targetRotation.x = 0f;
        targetRotation.z = 0f;
        m_Owner.rotation = Quaternion.RotateTowards(m_Owner.rotation, targetRotation, Time.deltaTime * 100);
    }
}

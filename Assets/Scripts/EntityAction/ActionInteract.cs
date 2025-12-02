using UnityEngine;


public enum InteractType
{
    PickUpItem,
    EnteringCode,
    ClimbingLadder,
    UseVehicle
}
[System.Serializable]
public class ActionInteractProperties : EntityActionProperties
{
    [SerializeField] private Transform m_InteractTransform;

    public Transform InteractTransform => m_InteractTransform;
}

public class ActionInteract : EntityContextAction
{
    [SerializeField] protected Transform m_Owner;
    [SerializeField] private InteractType m_Type;

    protected new ActionInteractProperties Properties;

    public InteractType Type => m_Type;

    public override void SetProperties(EntityActionProperties props)
    {
        Properties = props as ActionInteractProperties;
    }
    /*public override void StartAction() // Задать в другом месте
    {
        if (IsCanStart == false) return;

        base.StartAction();
        
        StartCoroutine(MoveToInteractTransformNumerator());
    }
    IEnumerator MoveToInteractTransformNumerator()
    {
        while (Vector3.Distance(m_Owner.position, Properties.InteractTransform.position) > 0.1f)
        {
            m_Owner.position = Vector3.MoveTowards(m_Owner.position, Properties.InteractTransform.position, Time.deltaTime * m_MoveToInteractTransformSpeedRate);
            LookAt(Properties.InteractTransform.root.position);
            yield return new WaitForEndOfFrame();
        }
    }
    private void LookAt(Vector3 target)
    {
        Vector3 targetDir = m_Owner.position - target;
        Quaternion targetRotation = Quaternion.LookRotation(-targetDir, Vector3.up);
        targetRotation.x = 0f;
        targetRotation.z = 0f;
        m_Owner.rotation = Quaternion.RotateTowards(m_Owner.rotation, targetRotation, Time.deltaTime * 100);
    }*/
}

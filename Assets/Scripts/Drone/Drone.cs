using UnityEngine;

public class Drone : Destructible
{
    [Header("Main")]
    [SerializeField] private Transform m_MainMesh;
    [SerializeField] private Weapon[] m_Turrets;

    [Header("View")]
    [SerializeField] private GameObject[] m_MeshComponents;
    [SerializeField] private Renderer[] m_MeshRenderers;
    [SerializeField] private Material[] m_DeadMaterials;

    [Header("Movement")]
    [SerializeField] private float m_HoverAmplitude;
    [SerializeField] private float m_HoverSpeed;
    [SerializeField] private float m_TargetMoveSpeed;
    [SerializeField] private float m_RotationLerpRate;

    public Transform MainMesh => m_MainMesh;

    private void Update()
    {
        Hover();
    }

    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();

        enabled = false;

        for (int i = 0; i < m_MeshComponents.Length; ++i)
        {
            if (m_MeshComponents[i].GetComponent<Rigidbody>() == null)
                m_MeshComponents[i].AddComponent<Rigidbody>();
        }
        for (int i = 0; i < m_MeshRenderers.Length; ++i)
        {
            m_MeshRenderers[i].material = m_DeadMaterials[i];
        }
    }

    private void Hover()
    {
        m_MainMesh.position += new Vector3(0, Mathf.Sin(Time.time * m_HoverAmplitude) * m_HoverSpeed * Time.deltaTime, 0);
    }
    
    // public API

    public void LookAt(Vector3 target)
    {
        Vector3 targetDir = target - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * m_RotationLerpRate);
    }
    public void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * m_TargetMoveSpeed);
    }
    public void Fire(Vector3 target)
    {
        if (target != null)
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].FirePointLookAt(target);
                m_Turrets[i].Fire();
            }
    }
}

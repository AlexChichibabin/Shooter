using System.Collections;
using UnityEngine;

[System.Serializable]
public class WheelAxle
{
    [SerializeField] private WheelCollider m_LeftWheelCollider;
    [SerializeField] private WheelCollider m_RightWheelCollider;
    [SerializeField] private Transform m_LeftWheelMesh;
    [SerializeField] private Transform m_RightWheelMesh;
    [SerializeField] private bool m_Motor;
    [SerializeField] private bool m_Steering;

    public bool Motor => m_Motor;
    public bool Steering => m_Steering;

    public void SetTorque(float torque)
    {
        if(m_Motor == false) return;

        m_LeftWheelCollider.motorTorque = torque;
        m_RightWheelCollider.motorTorque = torque;
    }
    public void SetBreak(float brakeTorque)
    {
        m_LeftWheelCollider.brakeTorque = brakeTorque;
        m_RightWheelCollider.brakeTorque = brakeTorque;
    }
    public void SetSteerAngle(float angle)
    {
        if (m_Steering == false) return;

        m_LeftWheelCollider.steerAngle = angle;
        m_RightWheelCollider.steerAngle = angle;
    }
    public void UpdateMeshTransform()
    {
        UpdateWheelTransform(m_LeftWheelCollider, ref m_LeftWheelMesh);
        UpdateWheelTransform(m_RightWheelCollider, ref m_RightWheelMesh);
    }
    private void UpdateWheelTransform(WheelCollider collider, ref Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);

        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
[RequireComponent(typeof(Rigidbody))]
public class WheeledVehicle : Vehicle
{
    [SerializeField] private WheelAxle[] m_WheelAxles;
    [SerializeField] private float m_MaxMotorTorque;
    [SerializeField] private float m_BrakeTorque;
    [SerializeField] private float m_MaxSteerAngle;
    

    private Rigidbody m_Rigidbody;

    public override float LinearVelocity => m_Rigidbody? m_Rigidbody.linearVelocity.magnitude : 0;

    protected override void Start()
    {
        base.Start();
        m_Rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(SetStartKinematicNum());
    }
    private void FixedUpdate()
    {
        float targetMotor = m_MaxMotorTorque * TargetInputControl.z;
        float brakeTorque = m_BrakeTorque * TargetInputControl.y;
        float steering = m_MaxSteerAngle * TargetInputControl.x;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            if (brakeTorque == 0 && LinearVelocity < m_MaxLinearVelocity)
            {
                m_WheelAxles[i].SetBreak(0);
                m_WheelAxles[i].SetTorque(targetMotor);
            }

            if (LinearVelocity > m_MaxLinearVelocity)
            {
                m_WheelAxles[i].SetBreak(m_BrakeTorque * 0.2f);
            }
            else
            {
                m_WheelAxles[i].SetBreak(brakeTorque);
            }
            
            m_WheelAxles[i].SetSteerAngle(steering);
            m_WheelAxles[i].UpdateMeshTransform();
            
            m_WheelAxles[i].SetBreak(brakeTorque);
        }
    }
    IEnumerator SetStartKinematicNum()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody>().isKinematic = true;
    }
}

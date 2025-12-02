using UnityEngine;

public class Turret : Weapon
{
    [SerializeField] private Transform m_TurretArm;
    [SerializeField] private Transform m_TurretGun;
    [SerializeField] private VehicleInputController m_Controller;
    [SerializeField] private Transform m_Aim;
    [SerializeField] private float m_VerticalRotationSpeed;
    [SerializeField] private float m_HorizontalRotationSpeed;

    protected override void Update()
    {
        base.Update();
        if (m_Aim != null)
        {
            m_TurretArm.rotation = UpdateHorizontalTurretAiming(m_Aim, m_TurretArm, m_HorizontalRotationSpeed);
            m_TurretGun.rotation = UpdateVerticalTurretAiming(m_Aim, m_TurretGun, m_VerticalRotationSpeed);
        }
    }
    public void SetAim(Transform aim)
    {
        m_Aim = aim;
    }
    private Quaternion UpdateHorizontalTurretAiming(Transform aim, Transform rotatedTransform, float speed)
    {
        Vector3 aimVector = aim.position - rotatedTransform.position;
        aimVector.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(aimVector, rotatedTransform.up);
        return Quaternion.RotateTowards(rotatedTransform.rotation, targetRotation, Time.deltaTime * speed);
    }
    private Quaternion UpdateVerticalTurretAiming(Transform aim, Transform rotatedTransform, float speed)
    {
        Vector3 aimVector = aim.position - rotatedTransform.position;
        aimVector.y = 0;
        float angle = Vector3.Angle(aimVector, m_Aim.position - m_TurretGun.position);
        if (0 <= (m_Aim.position - m_TurretGun.position).y)
            angle = -angle;
        Quaternion targetRotation = Quaternion.Euler(angle, m_TurretArm.eulerAngles.y, m_TurretArm.eulerAngles.z);
        return Quaternion.RotateTowards(rotatedTransform.rotation, targetRotation, Time.deltaTime * speed);
    }
}

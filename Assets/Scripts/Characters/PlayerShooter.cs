using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Weapon m_Weapon;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private RectTransform m_ImageSigh;

    public void Shoot()
    {
        RaycastHit hit;

        Ray ray = m_Camera.ScreenPointToRay(m_ImageSigh.position);

        if (Physics.Raycast(ray, out hit, 1000) == true)
        {
            if (hit.collider.isTrigger == true)
            {
                Physics.Raycast(hit.point + ray.direction, ray.direction, out hit, 1000);
            }
            m_Weapon.FirePointLookAt(hit.point);
        }

        if (m_Weapon.CanFire == true)
        {
            m_Weapon.Fire();
        }
    }
}

using UnityEngine;

public class ConverterCameraRotation : MonoBehaviour
{
    [SerializeField] private Transform m_Camera;
    [SerializeField] private Transform m_CameraLook;
    [SerializeField] private Vector3 m_LookOffset;
    [SerializeField] private float m_TopAngleLimit;
    [SerializeField] private float m_BottomAngleLimit;

    private void Update()
    {
        Vector3 spineRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_Camera.eulerAngles.x);

        if (spineRotation.z >= m_TopAngleLimit || spineRotation.z <= m_BottomAngleLimit)
        {
            transform.LookAt(m_CameraLook.position + m_LookOffset);
            transform.eulerAngles = spineRotation;
        }
    }
}

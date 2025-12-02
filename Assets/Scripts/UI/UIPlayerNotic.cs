using UnityEngine;

public class UIPlayerNotic : MonoBehaviour
{
    [SerializeField] private GameObject m_Hit;

    public void Show()
    {
        m_Hit.SetActive(true);
    }
    public void Hide()
    {
        m_Hit.SetActive(false);
    }

}

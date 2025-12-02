using UnityEngine;

public class GateAnimationController : MonoBehaviour
{
    private bool m_IsOpen = false;
    public bool IsOpen => m_IsOpen;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void ChangeDoorState()
    {
        m_IsOpen = !m_IsOpen;
        anim.SetBool("Is Open", m_IsOpen);
    }
}

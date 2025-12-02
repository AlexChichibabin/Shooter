using UnityEngine;

public class Scanner : MonoBehaviour
{
    private Collider m_Collider;
    private Destructible target;

    public Destructible Target => target;

    private void Start()
    {
        //m_Collider = GetComponent<Collider>();
        //Debug.Log(m_Collider);
    }
    private void Update()
    {

    }

}

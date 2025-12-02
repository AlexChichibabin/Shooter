using UnityEngine;

public class QuestReachedTrigger : Quest
{
    [SerializeField] private GameObject m_Owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != m_Owner)
        {
            Vehicle vehicle;
            if (other.transform.TryGetComponent(out vehicle) == true)
            {
                if (vehicle.Driver == null) return;
                if (vehicle.Driver.gameObject != m_Owner) return;
            } 
        }

        if(Completed != null) Completed.Invoke();
    }
    protected override void Update()
    {
        base.Update();

        if (m_Owner == null)
        {
            if (Player.Instance == null) return;
            m_Owner = Player.Instance.gameObject;
        }
    }
}

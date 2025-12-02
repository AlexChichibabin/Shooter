using UnityEngine;

public class VehicleUseTrigger : TriggerInteraction
{
    [SerializeField] private ActionUseVehicleProperties m_Properties;

    protected override void InitActionProperties()
    {
        m_Action.SetProperties(m_Properties);
    }
}

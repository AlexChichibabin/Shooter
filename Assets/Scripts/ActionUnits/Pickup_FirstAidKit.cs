using UnityEngine;

public class Pickup_FirstAidKit : TriggerInteraction
{
    protected override void OnStartAction(GameObject owner)
    {
        //m_Action.IsCanEnd = true;
    }
    protected override void OnEndAction(GameObject owner)
    {
        base.OnEndAction(owner);

        Destructible des = owner.transform.root.GetComponent<Destructible>();

        if (des != null) des.HealFull();

        Destroy(gameObject); 
    }
    
}

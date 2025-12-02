using UnityEngine;

public class SpreadShootRig : MonoBehaviour
{
    [SerializeField] private UnityEngine.Animations.Rigging.Rig m_SpreadRig;
    [SerializeField] private float m_ChangeWeightLerpRate;

    private bool isSpread = false;
    private float targetWeight;

    private void FixedUpdate()
    {
        if(isSpread == true) 
            m_SpreadRig.weight = Mathf.MoveTowards(m_SpreadRig.weight, targetWeight, Time.deltaTime * m_ChangeWeightLerpRate);
        Debug.Log(targetWeight);
        if (m_SpreadRig.weight == 1) targetWeight = 0f;
        else targetWeight = 1f;
    }
    public void Spread(bool active)
    {
        isSpread = active;
    }
}

using System.Collections;
using UnityEngine;

public class HeadLights : MonoBehaviour
{
    [SerializeField] private float lightIntensityLerpRate = 5.0f;

    private Light[] m_Lights;

    private bool isOn;
    private float defaultIntesity;

    private void Awake()
    {
        m_Lights = new Light[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            m_Lights[i] = transform.GetChild(i).GetComponent<Light>();
        }
        if(m_Lights[0] != null) defaultIntesity = m_Lights[0].intensity;

        SetHeadLightsState(false);
    }
    public void SetHeadLightsState(bool state)
    {
        if (m_Lights == null || m_Lights.Length == 0) return;
        
        isOn = state;
        float targetIntensity = defaultIntesity;
        if (isOn == false) targetIntensity = 0;

        for (int i = 0; i < m_Lights.Length; i++)
        {
            StartCoroutine(ChangeHeadLightsStateNumerator(m_Lights[i], targetIntensity));
        }
    }
    IEnumerator ChangeHeadLightsStateNumerator(Light light, float targetIntensity)
    {
        while (Mathf.Approximately(light.intensity, targetIntensity) != true)
        {
            light.intensity = Mathf.MoveTowards(light.intensity, targetIntensity, Time.deltaTime * lightIntensityLerpRate);
            yield return new WaitForEndOfFrame();
        }
    }
}

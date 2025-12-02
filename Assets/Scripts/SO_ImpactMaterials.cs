using UnityEngine;

[CreateAssetMenu(fileName = "SO_Materials", menuName = "Scriptable Objects/SO_Materials")]
public class SO_ImpactMaterials : ScriptableObject
{
    [SerializeField] private PhysicsMaterial[] m_PhysicsMaterials;
    [SerializeField] private ImpactEffect[] m_Impacts;
    [SerializeField] private so_Sounds[] m_Sounds;

    public bool GetImpactPrefabByMaterial(PhysicsMaterial mat, out ImpactEffect impact, out AudioClip clip)
    {
        for (int i = 0; i < m_PhysicsMaterials.Length; i++)
        {
            if (m_PhysicsMaterials[i].dynamicFriction == mat.dynamicFriction && m_PhysicsMaterials[i].staticFriction == mat.staticFriction)
            {
                impact = m_Impacts[i];
                clip = m_Sounds[i].GetRandomAudio();
                return true;
            }
        }
        impact = null;
        clip = null;
        return false;
    }
}

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Surface : MonoBehaviour
{
    [SerializeField] ImpactType m_ImpactType;
    public ImpactType Type => m_ImpactType;

    [ContextMenu("AddToAllObjects")]
    public void AddToAllObjects()
    {
        Transform[] allTransforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);

        for (int i = 0; i < allTransforms.Length; i++)
        {
            if (allTransforms[i].GetComponent<Collider>() != null)
            {
                if (allTransforms[i].GetComponent<Surface>() == null)
                {
                    allTransforms[i].gameObject.AddComponent<Surface>();
                }
            }
        }
    }
}

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CubeArea : MonoBehaviour
{
    [SerializeField] private Vector3 Area;

    public void SetArea(Vector3 area) => Area = area;
    public Vector3 GetRandomInsideZone()
    {
        Vector3 result = transform.localPosition;

        result.x += Random.Range(-Area.x / 2, Area.x / 2); 
        result.y += Random.Range(-Area.y / 2, Area.y / 2); 
        result.z += Random.Range(-Area.z / 2, Area.z / 2);

        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Area);
    }
#endif
}

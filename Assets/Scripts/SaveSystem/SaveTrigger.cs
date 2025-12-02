using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SaveTrigger : MonoBehaviour
{
    private bool isActive = true;

    SceneSerializer serializer;
    private void Awake()
    {
        serializer = FindAnyObjectByType<SceneSerializer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isActive == false) return;

        if (other.transform.root.GetComponent<Player>() == null) return;
        if (serializer == null)
        {
            serializer = FindAnyObjectByType<SceneSerializer>();
            return;
        }

        serializer.SaveScene();

        isActive = false;
    }
}

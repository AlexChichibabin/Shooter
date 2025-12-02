using UnityEngine;
using UnityEngine.UI;

public class UIRespawnButton : MonoBehaviour
{
    SceneSerializer serializer;
    Button button;


    private void Awake()
    {
        serializer = FindAnyObjectByType<SceneSerializer>();
        button = GetComponent<Button>();
        if (button == null) return;
        button.onClick.AddListener(OnClick);
        gameObject.SetActive(false);
    }

    private void OnClick()
    {
        if (button == null) return;
        serializer.LoadScene();
    }
}

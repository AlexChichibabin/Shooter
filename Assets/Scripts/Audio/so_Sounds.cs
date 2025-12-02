using UnityEngine;

[CreateAssetMenu(fileName = "SO_Sounds", menuName = "Scriptable Objects/SO_Sounds")]
public class so_Sounds : ScriptableObject
{
    [SerializeField] private AudioClip[] audioClips;

    public AudioClip GetRandomAudio()
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }
}

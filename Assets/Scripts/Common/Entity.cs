using UnityEngine;


/// <summary>
/// base class for all interactive game objects on scene
/// </summary>
public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// object name for user
    /// </summary>
    [SerializeField] private string m_Nickname;
    public string Nickname => m_Nickname;
}


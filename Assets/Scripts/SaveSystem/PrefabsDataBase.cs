using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsDataBase", menuName = "Scriptable Objects/PrefabsDataBase")]
public class PrefabsDataBase : ScriptableObject
{
    public Entity PlayerPrefab;
    public List<Entity> AllPrefabs;

    public GameObject CreateEntityFromId(long id)
    {
        foreach (var entity in AllPrefabs)
        {
            if((entity is ISerializableEntity) == false) continue;

            if ((entity as ISerializableEntity).EntityId == id)
            {
                return Instantiate(entity.gameObject);
            }
        }
        return null;
    }
    public bool PlayerID(long Id)
    {
        return Id == (PlayerPrefab as ISerializableEntity).EntityId;
    }
    public GameObject CreatePlayer()
    {
        return Instantiate(PlayerPrefab.gameObject);
    }
}

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SceneSerializer : MonoBehaviour
{
    [System.Serializable]
    public class SceneObjectState
    {
        public int sceneId;
        public long entityId;
        public string state;
    }

    [SerializeField] private PrefabsDataBase m_PrefabsDataBase;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveToFile("test.dat");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadFromFile("test.dat");
            //Time.timeScale = 0f;
        }
    }

    public void SaveScene()
    {
        SaveToFile("test.dat");
    }
    public void LoadScene()
    {
        LoadFromFile("test.dat");
    }


    private void SaveToFile(string path)
    {
        List<SceneObjectState> savedObjects = new List<SceneObjectState>();

        foreach (var entity in FindObjectsByType<Entity>(FindObjectsSortMode.None))
        {
            ISerializableEntity serializableEntity = entity as ISerializableEntity;

            if(serializableEntity == null) continue;
            if(serializableEntity.IsSerializable() == false) continue;

            SceneObjectState s = new SceneObjectState();

            s.entityId = serializableEntity.EntityId;
            s.state = serializableEntity.SerializeState();

            savedObjects.Add(s);
        }
        if (savedObjects.Count == 0)
        {
            Debug.Log("List saved objects is empty!");
            return;
        }


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" +  path);

        bf.Serialize(file, savedObjects);

        file.Close();

        Debug.Log("Scene saved! File path: " + Application.persistentDataPath + "/" + path);
    }
    private void LoadFromFile(string path)
    {
        if(Player.Instance != null) Player.Instance.Destroy();
        if(Camera.main != null) Destroy(Camera.main.gameObject);

        foreach (var entity in FindObjectsByType<Entity>(FindObjectsSortMode.None))
        {
            Destroy(entity.gameObject);
        }
        List<SceneObjectState> loadedObjects = new List<SceneObjectState>();

        if (File.Exists(Application.persistentDataPath + "/" + path)) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + path, FileMode.Open);

            loadedObjects = (List<SceneObjectState>)bf.Deserialize(file);
            file.Close();
        }

        foreach (var v in loadedObjects)
        {
            if (m_PrefabsDataBase.PlayerID(v.entityId) == false) continue;

            GameObject p = m_PrefabsDataBase.CreatePlayer();
            if (p != null) p.GetComponent<ISerializableEntity>().DeserializeState(v.state);
            loadedObjects.Remove(v);
            break;
        }

        foreach (var v in loadedObjects)
        {
            GameObject go = m_PrefabsDataBase.CreateEntityFromId(v.entityId);

            if(go!= null) go.GetComponent<ISerializableEntity>().DeserializeState(v.state);
        }
        Debug.Log("Scene loaded! File path: " + Application.persistentDataPath + "/" + path);
    }
}

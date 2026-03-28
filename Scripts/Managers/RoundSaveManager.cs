using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SavedBuilding
{
    public string type;
    public float x, y, z;
}

[Serializable]
public class RoundSaveData
{
    public List<SavedBuilding> buildings = new List<SavedBuilding>();
}

public class RoundSaveManager : MonoBehaviour
{
    public static RoundSaveManager Instance;

    private string savePath;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        savePath = Application.persistentDataPath + "/roundSave.json";
    }

    public void Save(List<GameObject> buildings)
    {
        RoundSaveData data = new RoundSaveData();

        foreach (var b in buildings)
        {
            if (b == null) continue;

            SavedBuilding sb = new SavedBuilding();
            sb.type = b.name.Replace("(Clone)", ""); // important
            sb.x = b.transform.position.x;
            sb.y = b.transform.position.y;
            sb.z = b.transform.position.z;

            data.buildings.Add(sb);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved surviving buildings: " + data.buildings.Count);
    }

    public RoundSaveData Load()
    {
        if (!File.Exists(savePath)) return null;

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<RoundSaveData>(json);
    }

    public void Clear()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
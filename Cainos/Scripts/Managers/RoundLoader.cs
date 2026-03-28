using UnityEngine;

public class RoundLoader : MonoBehaviour
{
    public GameObject[] buildingPrefabs;

    void Start()
    {

        if (!GameManager.Instance.isNewGame)
        {
            LoadBuildings();
        }
    }

    void LoadBuildings()
    {
        var data = RoundSaveManager.Instance.Load();

        if (data == null) return;

        foreach (var b in data.buildings)
        {
            GameObject prefab = GetPrefab(b.type);

            if (prefab != null)
            {
                Vector3 pos = new Vector3(b.x, b.y, b.z);
                GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

                // IMPORTANT: register again so it can be destroyed next round
                PlacedBuildingTracker.Instance.RegisterBuilding(obj);
            }
        }
    }

    GameObject GetPrefab(string name)
    {
        foreach (var prefab in buildingPrefabs)
        {
            if (prefab.name == name)
                return prefab;
        }
        return null;
    }
}
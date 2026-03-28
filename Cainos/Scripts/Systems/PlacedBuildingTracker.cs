using UnityEngine;
using System.Collections.Generic;

public class PlacedBuildingTracker : MonoBehaviour
{
    public static PlacedBuildingTracker Instance { get; private set; }

    private List<GameObject> placedThisRound = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterBuilding(GameObject building)
    {
        if (building != null)
        {
            placedThisRound.Add(building);
            Debug.Log("Registered building: " + building.name + " | Total: " + placedThisRound.Count);
        }
    }

    public void DestroyByScore(int roundScore)
    {
        //remove any buildings that were already destroyed
        placedThisRound.RemoveAll(b => b == null);

        float destroyedPercent;
        if (roundScore >= 220) destroyedPercent = 0.20f;
        else if (roundScore >= 170) destroyedPercent = 0.35f;
        else if (roundScore >= 120) destroyedPercent = 0.50f;
        else destroyedPercent = 0.70f;

        int destroyCount = Mathf.RoundToInt(placedThisRound.Count * destroyedPercent);

        for (int i = placedThisRound.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = placedThisRound[i];
            placedThisRound[i] = placedThisRound[j];
            placedThisRound[j] = temp;
        }

        //destroy the first destroyCount buildings
        for (int i = 0; i < destroyCount; i++)
        {
            if (placedThisRound[i] != null)
                Destroy(placedThisRound[i]);
        }

        placedThisRound.Clear();
    }

    //public void DestroyByScore(int roundScore)
    //{
    //    // Remove any buildings that were already destroyed
    //    placedThisRound.RemoveAll(b => b == null);

    //    Debug.Log("Buildings to destroy: " + placedThisRound.Count);

    //    for (int i = 0; i < placedThisRound.Count; i++)
    //    {
    //        if (placedThisRound[i] != null)
    //            Destroy(placedThisRound[i]);
    //    }

    //    placedThisRound.Clear();
    //}

    public void ResetRound()
    {
        placedThisRound.Clear();
    }
}
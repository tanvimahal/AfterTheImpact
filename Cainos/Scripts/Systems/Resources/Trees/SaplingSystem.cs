using UnityEngine;

public class SaplingSystem : MonoBehaviour
{
    public static SaplingSystem Instance;
    private int saplingCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddSaplings(int amount)
    {
        saplingCount += amount;
        SyncToResourceManager();
        UIEvents.OnResourceChanged?.Invoke();
        Debug.Log("Saplings: " + saplingCount);
    }

    public bool UseSapling(int amount)
    {
        if (saplingCount >= amount)
        {
            saplingCount -= amount;
            SyncToResourceManager();
            UIEvents.OnResourceChanged?.Invoke();
            Debug.Log("Saplings left: " + saplingCount);
            return true;
        }

        Debug.Log("Not enough saplings.");
        return false;
    }

    public int GetSaplingCount()
    {
        return saplingCount;
    }

    void SyncToResourceManager()
    {
        if (ResourceManager.Instance != null)
        {
            int current = ResourceManager.Instance.GetAmount(ResourceType.Sapling);
            int diff = saplingCount - current;

            if (diff > 0)
                ResourceManager.Instance.Add(ResourceType.Sapling, diff);
            else if (diff < 0)
                ResourceManager.Instance.Spend(ResourceType.Sapling, -diff);
        }
    }

    public void SetSaplingCount(int count)
    {
        saplingCount = count;
        SyncToResourceManager();
        UIEvents.OnResourceChanged?.Invoke();
        Debug.Log("Saplings set to: " + saplingCount);
    }
}
using UnityEngine;

public class WoodSystem : MonoBehaviour
{
    public static WoodSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void AddWood(int amount)
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager instance not found.");
            return;
        }

        ResourceManager.Instance.Add(ResourceType.Wood, amount);
        Debug.Log("Wood: " + ResourceManager.Instance.GetAmount(ResourceType.Wood));
    }

    public bool UseWood(int amount)
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager instance not found.");
            return false;
        }

        bool success = ResourceManager.Instance.Spend(ResourceType.Wood, amount);

        if (success)
            Debug.Log("Wood left: " + ResourceManager.Instance.GetAmount(ResourceType.Wood));
        else
            Debug.Log("Not enough wood.");

        return success;
    }

    public int GetWoodCount()
    {
        if (ResourceManager.Instance == null)
            return 0;

        return ResourceManager.Instance.GetAmount(ResourceType.Wood);
    }

    public void SetWoodCount(int count)
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager instance not found.");
            return;
        }

        int currentWood = ResourceManager.Instance.GetAmount(ResourceType.Wood);
        int difference = count - currentWood;

        if (difference > 0)
            ResourceManager.Instance.Add(ResourceType.Wood, difference);
        else if (difference < 0)
            ResourceManager.Instance.Spend(ResourceType.Wood, -difference);

        Debug.Log("Wood set to: " + ResourceManager.Instance.GetAmount(ResourceType.Wood));
    }
}
using UnityEngine;

public class FoodSystem : MonoBehaviour
{
    public static FoodSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void AddFood(int amount)
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager instance not found.");
            return;
        }

        ResourceManager.Instance.Add(ResourceType.Food, amount);
        Debug.Log("Food: " + ResourceManager.Instance.GetAmount(ResourceType.Food));
    }

    public int GetFoodCount()
    {
        if (ResourceManager.Instance == null)
            return 0;

        return ResourceManager.Instance.GetAmount(ResourceType.Food);
    }

    public void SetFoodCount(int count)
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager instance not found.");
            return;
        }

        int currentFood = ResourceManager.Instance.GetAmount(ResourceType.Food);
        int difference = count - currentFood;
        ResourceManager.Instance.Add(ResourceType.Food, difference);
    }
}
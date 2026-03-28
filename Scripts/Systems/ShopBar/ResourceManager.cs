using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [Header("Starting Resources")]
    public int startingWood = 0;
    public int startingSapling = 0;
    public int startingFood = 0;

    private Dictionary<ResourceType, int> resources;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wood,    startingWood    },
            { ResourceType.Sapling, startingSapling },
            { ResourceType.Food,    startingFood    },
        };

        ForceResetAllShopItems();
    }

    void ForceResetAllShopItems()
    {
        ShopItem[] allItems = Resources.FindObjectsOfTypeAll<ShopItem>();
        foreach (ShopItem item in allItems)
            item.placedThisRound = 0;
    }

    public int GetAmount(ResourceType type)
    {
        return resources.TryGetValue(type, out int v) ? v : 0;
    }

    public void Add(ResourceType type, int amount)
    {
        if (resources.ContainsKey(type))
            resources[type] += amount;

        UIEvents.OnResourceChanged?.Invoke();
    }

    public bool Spend(ResourceType type, int amount)
    {
        if (type == ResourceType.None) return true;
        if (GetAmount(type) < amount) return false;

        resources[type] -= amount;
        UIEvents.OnResourceChanged?.Invoke();
        return true;
    }
}
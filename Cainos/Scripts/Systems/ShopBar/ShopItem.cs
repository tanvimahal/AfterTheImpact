using UnityEngine;

public enum ShopItemCategory
{
    Nature,
    Structure,
    Decoration
}

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Item")]
public class ShopItem : ScriptableObject
{
    [Header("Display")]
    public string itemName;
    public Sprite icon;
    public GameObject prefab;

    [Header("Category")]
    public ShopItemCategory category;

    [Header("Cost")]
    public ResourceType costType;
    public int costAmount;

    [Header("Placement Limits")]
    public bool hasRoundLimit = false;
    public int maxPerRound = 5;

    [Header("XP")]
    public int xpReward = 10;

    [Header("Sustainability")]
    public float sustainabilityChange = 5f;

    [System.NonSerialized] public int placedThisRound = 0;

    public bool CanAfford(ResourceManager rm)
    {
        if (costType == ResourceType.None) return true;
        return rm.GetAmount(costType) >= costAmount;
    }

    public bool IsMaxed()
    {
        return hasRoundLimit && placedThisRound >= maxPerRound;
    }

    public void ResetRound()
    {
        placedThisRound = 0;
    }

    private void OnEnable()
    {
        placedThisRound = 0;
    }
}
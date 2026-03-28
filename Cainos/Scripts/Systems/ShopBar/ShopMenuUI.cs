using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ShopMenuUI : MonoBehaviour
{
    [Header("Layout")]
    public Transform itemContainer;
    public GameObject itemCardPrefab;

    [Header("Category Buttons")]
    public Button[] categoryButtons;

    [Header("Items")]
    public List<ShopItem> natureItems;
    public List<ShopItem> structureItems;
    public List<ShopItem> decorationItems;

    [Header("Resource Labels")]
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI saplingText;
    public TextMeshProUGUI foodText;

    [Header("Tab Colors")]
    public Color activeTabColor = new Color(1f, 1f, 1f, 1f);
    public Color inactiveTabColor = new Color(0.8f, 0.8f, 0.8f, 1f);

    private int currentCategory = 0;
    private ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        UIEvents.OnResourceChanged += RefreshResourceDisplay;
        RefreshResourceDisplay();

        for (int i = 0; i < categoryButtons.Length; i++)
        {
            int idx = i;
            categoryButtons[i].onClick.AddListener(() => SetCategory(idx));
        }

        StartCoroutine(DelayedStart());
    }

    void OnDestroy()
    {
        UIEvents.OnResourceChanged -= RefreshResourceDisplay;
    }

    IEnumerator DelayedStart()
    {
        yield return null;
        yield return null;
        ForceResetAllItems();
        SetCategory(0);
        StartCoroutine(ForceLayoutRefresh());
    }

    void ForceResetAllItems()
    {
        foreach (ShopItem item in natureItems)
            if (item != null) item.placedThisRound = 0;

        foreach (ShopItem item in structureItems)
            if (item != null) item.placedThisRound = 0;

        foreach (ShopItem item in decorationItems)
            if (item != null) item.placedThisRound = 0;
    }

    void SetCategory(int idx)
    {
        currentCategory = idx;

        List<ShopItem> items = idx switch
        {
            0 => natureItems,
            1 => structureItems,
            2 => decorationItems,
            _ => natureItems
        };

        for (int i = 0; i < categoryButtons.Length; i++)
        {
            var img = categoryButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = (i == idx) ? activeTabColor : inactiveTabColor;
        }

        RebuildCards(items);
    }

    void RebuildCards(List<ShopItem> items)
    {
        foreach (Transform child in itemContainer)
            Destroy(child.gameObject);

        foreach (ShopItem item in items)
        {
            if (item == null) continue;
            GameObject cardObj = Instantiate(itemCardPrefab, itemContainer);
            ShopCardUI card = cardObj.GetComponent<ShopCardUI>();
            if (card != null)
                card.Setup(item, OnCardClicked);
        }

        StartCoroutine(ForceLayoutRefresh());
    }

    private IEnumerator ForceLayoutRefresh()
    {
        yield return null;

        RectTransform contentRect = itemContainer as RectTransform;
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(0, 1);
        contentRect.pivot = new Vector2(0, 1);
        contentRect.anchoredPosition = new Vector2(0, 0);

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        if (scrollRect != null)
            scrollRect.horizontalNormalizedPosition = 0f;
    }

    void OnCardClicked(ShopItem item)
    {
        if (item.IsMaxed() || !item.CanAfford(ResourceManager.Instance)) return;
        UIEvents.OnItemSelected?.Invoke(item);
    }

    void RefreshResourceDisplay()
    {
        if (woodText != null && WoodSystem.Instance != null)
            woodText.text = WoodSystem.Instance.GetWoodCount().ToString();

        if (saplingText != null && SaplingSystem.Instance != null)
            saplingText.text = SaplingSystem.Instance.GetSaplingCount().ToString();

        if (foodText != null && ResourceManager.Instance != null)
            foodText.text = ResourceManager.Instance.GetAmount(ResourceType.Food).ToString();

        foreach (Transform child in itemContainer)
        {
            ShopCardUI card = child.GetComponent<ShopCardUI>();
            if (card != null) card.RefreshState();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopCardUI : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI costLabel;
    public TextMeshProUGUI limitLabel;
    public Image cardBackground;
    public Button button;

    [Header("Colors")]
    public Color normalColor = new Color(1f, 1f, 1f, 1f);
    public Color selectedColor = new Color(0.8f, 0.9f, 0.8f, 1f);
    public Color lockedColor = new Color(0.85f, 0.85f, 0.85f, 1f);

    private ShopItem item;
    private Action<ShopItem> onClick;
    private bool isSelected = false;

    public void Setup(ShopItem shopItem, Action<ShopItem> callback)
    {
        item = shopItem;
        onClick = callback;

        icon.sprite = item.icon;
        nameLabel.text = item.itemName;

        if (item.costType == ResourceType.None || item.costAmount == 0)
            costLabel.text = "Free";
        else
            costLabel.text = item.costAmount + " " + item.costType.ToString();

        if (item.hasRoundLimit)
        {
            limitLabel.gameObject.SetActive(true);
            limitLabel.text = item.placedThisRound + "/" + item.maxPerRound;
        }
        else
        {
            limitLabel.gameObject.SetActive(false);
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke(item));

        RefreshState();
    }

    public void RefreshState()
    {
        if (item == null) return;

        bool locked = item.IsMaxed() || !item.CanAfford(ResourceManager.Instance);

        button.interactable = !locked;

        if (locked)
            cardBackground.color = lockedColor;
        else if (isSelected)
            cardBackground.color = selectedColor;
        else
            cardBackground.color = normalColor;

        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
            cg.alpha = locked ? 0.5f : 1f;

        if (item.hasRoundLimit)
            limitLabel.text = item.placedThisRound + "/" + item.maxPerRound;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        RefreshState();
    }
}
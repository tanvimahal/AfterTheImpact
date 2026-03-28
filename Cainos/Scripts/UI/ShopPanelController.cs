using UnityEngine;
using TMPro;

public class ShopPanelController : MonoBehaviour
{
    public RectTransform shopPanel;

    [Header("Positions")]
    public Vector2 shownPosition;
    public Vector2 hiddenPosition;

    [Header("Toggle UI")]
    public TextMeshProUGUI toggleButtonText;

    private bool isOpen = false;

    void Start()
    {
        if (shopPanel == null)
            shopPanel = GetComponent<RectTransform>();

        shopPanel.anchoredPosition = hiddenPosition;
        isOpen = false;
        UpdateToggleText();
    }

    public void ShowShop()
    {
        shopPanel.anchoredPosition = shownPosition;
        isOpen = true;
        UpdateToggleText();
    }

    public void HideShop()
    {
        shopPanel.anchoredPosition = hiddenPosition;
        isOpen = false;
        UpdateToggleText();
    }

    public void ToggleShop()
    {
        if (isOpen)
            HideShop();
        else
            ShowShop();
    }

    void UpdateToggleText()
    {
        if (toggleButtonText != null)
            toggleButtonText.text = isOpen ? "▼" : "▲";
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
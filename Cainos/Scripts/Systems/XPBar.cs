using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;
    public Image fillImage;
    public TextMeshProUGUI levelText;

    [Header("Shop Slide")]
    public RectTransform shopBarRect;
    public float shopOpenHeight = 350f;
    public float shopClosedHeight = 0f;
    public float slideSpeed = 8f;

    [Header("Stacking")]
    public SustainabilityBar sustainabilityBar;
    public float stackOffset = 40f; // distance above sustainability bar

    [Header("Position Offsets")]
    public float heightOffset = 10f;
    public float closedYPosition = 140f;

    private bool shopOpen = true;
    private float targetShopHeight;

    void Start()
    {
        targetShopHeight = shopOpenHeight;
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 0;
        fillImage.color = Color.blue;

        XPManager.OnXPChanged += RefreshUI;
        RefreshUI();
    }

    void OnDestroy()
    {
        XPManager.OnXPChanged -= RefreshUI;
    }

    void Update()
    {
        if (shopBarRect != null)
        {
            RectTransform rt = GetComponent<RectTransform>();
            RectTransform sustainabilityRt = sustainabilityBar.GetComponent<RectTransform>();
            if (rt != null && sustainabilityRt != null)
            {
                Vector2 pos = rt.anchoredPosition;
                pos.y = Mathf.Lerp(pos.y, sustainabilityRt.anchoredPosition.y + stackOffset, Time.deltaTime * slideSpeed);
                rt.anchoredPosition = pos;
            }
        }
    }

    public void ToggleShop(bool isOpen)
    {
        shopOpen = isOpen;
        targetShopHeight = shopOpen ? shopOpenHeight : shopClosedHeight;
    }

    void RefreshUI()
    {
        slider.value = XPManager.Instance.GetPercent();
        if (levelText != null)
            levelText.text = "Lvl " + XPManager.Instance.CurrentLevel;
    }
}
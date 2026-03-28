using UnityEngine;
using UnityEngine.UI;

public class SustainabilityBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;
    public Image fillImage;

    [Header("Shop Reference")]
    public ShopPanelController shopPanelController;

    [Header("Bar Positions")]
    public RectTransform sustainabilityBarRect;
    public float openY = 430f;
    public float closedY = 100f;
    public float moveSpeed = 8f;

    void Start()
    {
        slider.maxValue = SustainabilityManager.Instance.maxSustainability;
        slider.value = SustainabilityManager.Instance.currentSustainability;
        SustainabilityManager.OnSustainabilityChanged += RefreshUI;
        RefreshUI();
    }

    void OnDestroy()
    {
        SustainabilityManager.OnSustainabilityChanged -= RefreshUI;
    }

    void Update()
    {
        if (shopPanelController != null && sustainabilityBarRect != null)
        {
            Vector2 pos = sustainabilityBarRect.anchoredPosition;
            float targetY = shopPanelController.IsOpen() ? openY : closedY;
            pos.y = Mathf.Lerp(pos.y, targetY, Time.unscaledDeltaTime * moveSpeed);
            sustainabilityBarRect.anchoredPosition = pos;
        }
    }

    void RefreshUI()
    {
        float val = SustainabilityManager.Instance.currentSustainability;
        slider.value = val;
        UpdateBarColor(SustainabilityManager.Instance.GetPercent());
    }

    void UpdateBarColor(float percent)
    {
        if (percent > 0.6f)
            fillImage.color = Color.green;
        else if (percent > 0.3f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }
}
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance { get; private set; }
    public int CurrentRoundXP { get; private set; }


    [Header("XP Settings")]
    public int xpPerLevel = 100;

    public int CurrentXP { get; private set; }
    public int CurrentLevel { get; private set; } = 1;

    public static event System.Action OnXPChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddXP(int amount)
    {
        CurrentXP += amount;
        CurrentRoundXP += amount;
        while (CurrentXP >= xpPerLevel)
        {
            CurrentXP -= xpPerLevel;
            CurrentLevel++;
        }
        OnXPChanged?.Invoke();
    }

    public float GetPercent()
    {
        return (float)CurrentXP / xpPerLevel;
    }
    public void ResetXP()
    {
        CurrentXP = 0;
        CurrentRoundXP = 0;
        OnXPChanged?.Invoke();
    }
}
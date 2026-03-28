using UnityEngine;

public class PassiveSustainability : MonoBehaviour
{
    public float amountPerTick = 3f;
    public float interval = 20f;

    void Start()
    {
        if (SustainabilityManager.Instance != null)
            SustainabilityManager.Instance.StartPassiveIncrease(amountPerTick, interval);
    }
}
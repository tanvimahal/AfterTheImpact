using UnityEngine;
using System.Collections;

public class SustainabilityManager : MonoBehaviour
{
    public static SustainabilityManager Instance { get; private set; }

    [Header("Settings")]
    public float maxSustainability = 100f;
    public float currentSustainability = 50f;

    [Header("Change Amounts")]
    public float plantTreeAmount = 10f;
    public float cutTreeAmount = 10f;
    public float killAnimalAmount = 10f;
    public float disasterAmount = 5f;

    public static System.Action OnSustainabilityChanged;

    private bool hasTriggeredGameOver = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void PlantTree() => Increase(plantTreeAmount);
    public void CutTree() => Decrease(cutTreeAmount);
    public void KillAnimal() => Decrease(killAnimalAmount);
    public void OnDisaster() => Decrease(disasterAmount);

    public void Increase(float amount)
    {
        currentSustainability = Mathf.Clamp(
            currentSustainability + amount, 0f, maxSustainability);

        OnSustainabilityChanged?.Invoke();
    }

    public void Decrease(float amount)
    {
        currentSustainability = Mathf.Clamp(
            currentSustainability - amount, 0f, maxSustainability);

        OnSustainabilityChanged?.Invoke();

        // GAME OVER TRIGGER
        if (currentSustainability <= 0f && !hasTriggeredGameOver)
        {
            hasTriggeredGameOver = true;

            Debug.Log("Sustainability reached 0 → Game Over");

            DisasterManager disasterManager = FindFirstObjectByType<DisasterManager>();

            if (disasterManager != null)
            {
                disasterManager.TriggerGameOverFromLowSustainability();
            }
            else
            {
                Debug.LogWarning("DisasterManager not found, returning to menu.");
                GameManager.Instance.ReturnToMenu();
            }
        }
    }

    public float GetPercent()
    {
        return currentSustainability / maxSustainability;
    }

    public void StartPassiveIncrease(float amount, float interval)
    {
        StartCoroutine(PassiveIncrease(amount, interval));
    }

    IEnumerator PassiveIncrease(float amount, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Increase(amount);
        }
    }
}
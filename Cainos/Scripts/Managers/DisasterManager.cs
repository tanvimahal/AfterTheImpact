using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DisasterManager : MonoBehaviour
{
    public float disasterInterval = 60f;
    private float timer;

    public Slider timeBar;
    public int disasterCycle = 0;
    public Text disasterCycleText;
    public GameObject disasterOverlay;
    public DatabaseAPI databaseAPI;
    public ShopPanelController shopPanelController;
    public int lastRoundScore = 0;

    [Header("Gameplay UI To Hide During Feedback")]
    public GameObject sustainabilityBar;
    public GameObject xpBar;

    [Header("Shop Items to Reset Each Round")]
    public List<ShopItem> allShopItems;

    private bool disasterInProgress = false;

    void Start()
    {
        disasterCycle = GameManager.Instance.loadedCycle;
        UpdateUI();
        timer = disasterInterval;

        if (timeBar != null)
        {
            timeBar.maxValue = disasterInterval;
            timeBar.value = disasterInterval;
        }

        SetStartingSustainability(disasterCycle);

        if (!GameManager.Instance.isNewGame)
        {
            FoodSystem.Instance.SetFoodCount(GameManager.Instance.food_count);
            SaplingSystem.Instance.SetSaplingCount(GameManager.Instance.sapling_count);
            WoodSystem.Instance.SetWoodCount(GameManager.Instance.wood_count);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (disasterInProgress) return;

        timer -= Time.deltaTime;

        if (timeBar != null)
            timeBar.value = timer;

        if (timer <= 0f)
            TriggerDisaster();
    }

    void UpdateUI()
    {
        if (disasterCycleText != null)
            disasterCycleText.text = "Disaster Cycle: " + disasterCycle;
    }

    void SetStartingSustainability(int round)
    {
        if (SustainabilityManager.Instance != null)
        {
            float startingSustainability = 75f - (round * 5f);
            startingSustainability = Mathf.Max(startingSustainability, 20f);
            SustainabilityManager.Instance.currentSustainability = startingSustainability;
            SustainabilityManager.OnSustainabilityChanged?.Invoke();
        }
    }

    void TriggerDisaster()
    {
        if (disasterInProgress) return;
        if (disasterOverlay != null && disasterOverlay.activeSelf) return;

        StartCoroutine(DisasterSequence());

        if (databaseAPI != null)
        {
            StartCoroutine(databaseAPI.SaveGame(
                GameManager.Instance.username,
                GameManager.Instance.password,
                disasterCycle,
                FoodSystem.Instance.GetFoodCount(),
                SaplingSystem.Instance.GetSaplingCount(),
                WoodSystem.Instance.GetWoodCount(),
                lastRoundScore,
                GameStatsManager.Instance.GetTreesCutTotal(),//total_trees_cut,
                GameStatsManager.Instance.TreesPlacedTotal,//total_trees_planted,
                GameStatsManager.Instance.AnimalsKilledTotal,//total_animals_killed,
                GameStatsManager.Instance.BuildingsPlacedTotal//total_buildings_built
            ));
        }
    }

    IEnumerator DisasterSequence()
    {
        disasterInProgress = true;
        disasterCycle++;

        Debug.Log("DisasterSequence started. Cycle: " + disasterCycle);

        UpdateUI();

        // Hide bars
        if (sustainabilityBar != null)
            sustainabilityBar.SetActive(false);

        if (xpBar != null)
            xpBar.SetActive(false);

        if (disasterOverlay != null)
            disasterOverlay.SetActive(true);

        if (shopPanelController != null)
            shopPanelController.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        if (disasterOverlay != null)
            disasterOverlay.SetActive(false);

        // Calculate score
        if (XPManager.Instance != null && SustainabilityManager.Instance != null)
        {
            int currentRoundXP = XPManager.Instance.CurrentRoundXP;
            float currentSustainability = SustainabilityManager.Instance.currentSustainability;
            lastRoundScore = Mathf.RoundToInt(currentRoundXP + (currentSustainability * 2f));

            Debug.Log("Round " + disasterCycle + " Score: " + lastRoundScore);
        }

        // WIN CONDITION
        if (disasterCycle >= 5)
        {
            Debug.Log("Showing FINAL WIN screen");

            if (FinalFeedbackUI.Instance != null)
                FinalFeedbackUI.Instance.ShowFinalFeedback(disasterCycle, true);
            else
                Debug.LogWarning("FinalFeedbackUI instance not found.");

            yield break;
        }

        // NORMAL ROUND FEEDBACK
        if (RoundFeedbackUI.Instance != null)
        {
            Debug.Log("Showing ROUND feedback");
            RoundFeedbackUI.Instance.ShowRoundFeedback(disasterCycle);
        }
        else
        {
            Debug.LogWarning("RoundFeedbackUI instance not found.");

            disasterInProgress = false;
            timer = disasterInterval;

            if (timeBar != null)
                timeBar.value = timer;

            if (sustainabilityBar != null)
                sustainabilityBar.SetActive(true);

            if (xpBar != null)
                xpBar.SetActive(true);
        }
    }

    // 
    public void ContinueAfterRoundFeedback()
    {
        Debug.Log("ContinueAfterRoundFeedback called");

        if (RoundFeedbackUI.Instance != null)
            RoundFeedbackUI.Instance.HideRoundFeedback();

        // Show bars again
        if (sustainabilityBar != null)
            sustainabilityBar.SetActive(true);

        if (xpBar != null)
            xpBar.SetActive(true);

        if (XPManager.Instance != null)
            XPManager.Instance.ResetXP();

        if (GameStatsManager.Instance != null)
            GameStatsManager.Instance.ResetRoundStats();

        ResetShopItems();
        SetStartingSustainability(disasterCycle);
        SustainabilityManager.Instance?.OnDisaster();

        timer = disasterInterval;

        if (timeBar != null)
            timeBar.value = timer;

        if (shopPanelController != null)
        {
            shopPanelController.gameObject.SetActive(true);
            shopPanelController.HideShop();
        }

        Time.timeScale = 1f;
        disasterInProgress = false;

        Debug.Log("Round resumed");
    }

    //  LOSS CONDITION
    public void TriggerGameOverFromLowSustainability()
    {
        if (disasterInProgress) return;

        Debug.Log("Game Over triggered from low sustainability");

        disasterInProgress = true;

        if (sustainabilityBar != null)
            sustainabilityBar.SetActive(false);

        if (xpBar != null)
            xpBar.SetActive(false);

        if (shopPanelController != null)
            shopPanelController.gameObject.SetActive(false);

        int roundsSurvived = disasterCycle;

        if (FinalFeedbackUI.Instance != null)
        {
            FinalFeedbackUI.Instance.ShowFinalFeedback(roundsSurvived, false);
        }
        else
        {
            Debug.LogWarning("FinalFeedbackUI instance not found.");
            GameManager.Instance.ReturnToMenu();
        }
    }

    void ResetShopItems()
    {
        foreach (ShopItem item in allShopItems)
        {
            if (item != null)
                item.placedThisRound = 0;
        }

        UIEvents.OnResourceChanged?.Invoke();
    }
}
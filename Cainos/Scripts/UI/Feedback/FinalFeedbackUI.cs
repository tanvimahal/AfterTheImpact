using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalFeedbackUI : MonoBehaviour
{
    public static FinalFeedbackUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject panelContent;
    public TextMeshProUGUI congratulationsText;
    public TextMeshProUGUI cyclesSurvivedText;
    public TextMeshProUGUI finalSummaryText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI finalGradeText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Start hidden
        if (panelContent != null)
            panelContent.SetActive(false);
    }

    public void ShowFinalFeedback(int roundsCompleted, bool playerWon)
    {
        if (GameStatsManager.Instance == null)
        {
            Debug.LogWarning("GameStatsManager instance not found.");
            return;
        }

        var stats = GameStatsManager.Instance;

        // Show panel
        if (panelContent != null)
            panelContent.SetActive(true);

        // Title
        if (congratulationsText != null)
        {
            congratulationsText.text = playerWon ? "Congratulations!" : "Game Over";
        }

        // Cycles survived
        if (cyclesSurvivedText != null)
        {
            if (playerWon)
                cyclesSurvivedText.text = "You survived all disaster cycles.";
            else{
                int shownCycles = Mathf.Max(0, roundsCompleted + 1);
                cyclesSurvivedText.text = $"Your sustainability collapsed during cycle {shownCycles}.";
            }
        }

        // Summary title
        if (finalSummaryText != null)
        {
            int shownRounds = Mathf.Max(0, roundsCompleted + 1);
            finalSummaryText.text = $"Summary After {shownRounds} Round(s):";
        }
        // Stats
        if (statsText != null)
        {
            statsText.text =
                $"Trees Planted: {stats.TreesPlacedTotal}\n" +
                $"Trees Cut: {stats.TreesCutTotal}\n" +
                $"Buildings Placed: {stats.BuildingsPlacedTotal}\n" +
                $"Animals Killed: {stats.AnimalsKilledTotal}";
        }

        // Grade (ONLY letter now)
        if (finalGradeText != null)
            finalGradeText.text = GetFinalGradeText(stats, playerWon);

        // Pause game
        Time.timeScale = 0f;
    }

    public void HideFinalFeedback()
    {
        if (panelContent != null)
            panelContent.SetActive(false);

        Time.timeScale = 1f;
    }

    // Grade logic
    private string GetFinalGradeText(GameStatsManager stats, bool playerWon)
    {
        int score = CalculateFinalScore(stats, playerWon);
        string grade = GetLetterGrade(score);

        return $"Final Grade: {grade}";
    }

    private int CalculateFinalScore(GameStatsManager stats, bool playerWon)
    {
        int score = 100;

        // Positive
        score += stats.TreesPlacedTotal * 3;

        // Negative
        score -= stats.TreesCutTotal * 2;
        score -= stats.AnimalsKilledTotal * 4;
        score -= stats.BuildingsPlacedTotal * 1;

        // Extra penalty if player lost early
        if (!playerWon)
            score -= 20;

        return Mathf.Clamp(score, 0, 120);
    }

    private string GetLetterGrade(int score)
    {
        if (score >= 95) return "A";
        if (score >= 80) return "B";
        if (score >= 65) return "C";
        if (score >= 50) return "D";
        return "F";
    }

    // Buttons

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ReturnToMenu();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
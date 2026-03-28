using UnityEngine;
using TMPro;

public class RoundFeedbackUI : MonoBehaviour
{
    public static RoundFeedbackUI Instance { get; private set; }

    [Header("UI References")]
    public GameObject panelContent;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI didYouKnowText;

    [Header("Did You Know Facts")]
    [TextArea(2, 5)]
    public string[] sustainableFacts;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (panelContent != null)
            panelContent.SetActive(false);
    }

    public void ShowRoundFeedback(int roundNumber)
    {
        if (GameStatsManager.Instance == null)
        {
            Debug.LogWarning("GameStatsManager instance not found.");
            return;
        }

        var stats = GameStatsManager.Instance;

        if (panelContent != null)
            panelContent.SetActive(true);

        if (roundText != null)
            roundText.text = $"Round {roundNumber} Complete";

        if (statsText != null)
        {
            statsText.text =
                $"Trees Planted: {stats.TreesPlacedThisRound}\n" +
                $"Trees Cut: {stats.TreesCutThisRound}\n" +
                $"Buildings Placed: {stats.BuildingsPlacedThisRound}\n" +
                $"Animals Killed: {stats.AnimalsKilledThisRound}";
        }

        if (feedbackText != null)
            feedbackText.text = GetRoundComment(stats);

        if (didYouKnowText != null)
            didYouKnowText.text = GetRandomFact();

        Time.timeScale = 0f;
    }

    public void HideRoundFeedback()
    {
        if (panelContent != null)
            panelContent.SetActive(false);

        Time.timeScale = 1f;
    }

    private string GetRoundComment(GameStatsManager stats)
    {
        int planted = stats.TreesPlacedThisRound;
        int cut = stats.TreesCutThisRound;
        int animals = stats.AnimalsKilledThisRound;
        int buildings = stats.BuildingsPlacedThisRound;

        // 🌱 VERY GOOD (eco positive)
        if (planted > cut && animals <= 1 && buildings <= 2)
        {
            string[] comments =
            {
                "Excellent work. You restored more nature than you consumed.",
                "Your ecosystem is thriving thanks to your decisions.",
                "Great balance. Nature is recovering under your leadership.",
                "You are building sustainably. Keep it up.",
                "This round had a positive environmental impact."
            };
            return GetRandomFromArray(comments);
        }

        // 🌿 GOOD (slightly positive)
        if (planted >= cut && animals <= 2)
        {
            string[] comments =
            {
                "Good job. You kept a healthy balance this round.",
                "Your choices are helping maintain stability.",
                "You're managing resources responsibly so far.",
                "This round stayed mostly sustainable.",
                "You're on the right path. Keep improving."
            };
            return GetRandomFromArray(comments);
        }

        // ⚖️ BALANCED
        if (planted == cut)
        {
            string[] comments =
            {
                "Your impact was balanced this round.",
                "You maintained equilibrium, but no progress was made.",
                "Stable, but consider improving sustainability.",
                "A neutral round. Try to restore more next time.",
                "You held things steady, but growth is still needed."
            };
            return GetRandomFromArray(comments);
        }

        // 🏗️ BUILDING HEAVY (no nature support)
        if (buildings > 0 && planted == 0)
        {
            string[] comments =
            {
                "You expanded quickly, but ignored nature.",
                "Growth came at the cost of sustainability.",
                "Your settlement is growing, but the environment is suffering.",
                "Consider balancing expansion with restoration.",
                "Development without care can lead to collapse."
            };
            return GetRandomFromArray(comments);
        }

        // 🪓 OVER-EXPLOITATION (cutting too much)
        if (cut > planted && animals <= 2)
        {
            string[] comments =
            {
                "You consumed more than you restored.",
                "Resources are being depleted faster than they recover.",
                "Careful. You're putting pressure on the ecosystem.",
                "Nature is declining under your actions.",
                "Try planting more to balance your resource use."
            };
            return GetRandomFromArray(comments);
        }

        // 🐄 ANIMAL DAMAGE (overhunting)
        if (animals > 2)
        {
            string[] comments =
            {
                "Overhunting is damaging the ecosystem.",
                "Animal populations are declining too quickly.",
                "Your hunting is disrupting ecological balance.",
                "Be careful. Wildlife loss affects sustainability.",
                "Too many animals were killed this round."
            };
            return GetRandomFromArray(comments);
        }

        // 💀 VERY BAD (everything negative)
        if (cut > planted && animals > 2 && buildings > 2)
        {
            string[] comments =
            {
                "Severe environmental damage this round.",
                "Your actions are destabilizing the ecosystem.",
                "The environment is collapsing under pressure.",
                "You are overexploiting all available resources.",
                "This level of impact is not sustainable."
            };
            return GetRandomFromArray(comments);
        }

        // fallback
        return "This round had mixed results. Try improving your balance.";
    }

    private string GetRandomFact()
    {
        if (sustainableFacts == null || sustainableFacts.Length == 0)
            return "Did you know? Planting trees helps restore habitats and improve air quality.";

        int randomIndex = Random.Range(0, sustainableFacts.Length);
        return sustainableFacts[randomIndex];
    }

    private string GetRandomFromArray(string[] array)
    {
        if (array == null || array.Length == 0)
            return "";

        int index = Random.Range(0, array.Length);
        return array[index];
    }
}
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }
    public int TreesPlacedThisRound { get; private set; }
    public int TreesCutThisRound { get; private set; }
    public int BuildingsPlacedThisRound { get; private set; }
    public int AnimalsKilledThisRound { get; private set; }
    public int TreesPlacedTotal { get; private set; }
    public int TreesCutTotal { get; private set; }
    public int BuildingsPlacedTotal { get; private set; }
    public int AnimalsKilledTotal { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void RecordTreePlaced()
    {
        TreesPlacedThisRound++;
        TreesPlacedTotal++;
        LogStats();
    }

    public void RecordTreeCut()
    {
        TreesCutThisRound++;
        TreesCutTotal++;
        LogStats();
    }

    public void RecordBuildingPlaced()
    {
        BuildingsPlacedThisRound++;
        BuildingsPlacedTotal++;
        LogStats();
    }

    public void RecordAnimalKilled()
    {
        AnimalsKilledThisRound++;
        AnimalsKilledTotal++;
        LogStats();
    }

    public void ResetRoundStats()
    {
        TreesPlacedThisRound = 0;
        TreesCutThisRound = 0;
        BuildingsPlacedThisRound = 0;
        AnimalsKilledThisRound = 0;
        Debug.Log("Round stats reset.");
    }

    private void LogStats()
    {
        Debug.Log($"[Stats] Trees Placed: {TreesPlacedThisRound} | " +
                  $"Trees Cut: {TreesCutThisRound} | " +
                  $"Buildings: {BuildingsPlacedThisRound} | " +
                  $"Animals Killed: {AnimalsKilledThisRound}");
    }

    public string GetRoundSummary(int roundNumber)
    {
        return $"Round {roundNumber} Summary:\n" +
               $"Trees Planted: {TreesPlacedThisRound}\n" +
               $"Trees Cut: {TreesCutThisRound}\n" +
               $"Buildings Placed: {BuildingsPlacedThisRound}\n" +
               $"Animals Killed: {AnimalsKilledThisRound}";
    }

    public string GetTotalSummary(int roundsCompleted)
    {
        return $"Total After {roundsCompleted} Rounds:\n" +
               $"Trees Planted: {TreesPlacedTotal}\n" +
               $"Trees Cut: {TreesCutTotal}\n" +
               $"Buildings Placed: {BuildingsPlacedTotal}\n" +
               $"Animals Killed: {AnimalsKilledTotal}";
    }

    public void SetTreesCutTotal(int count)
    {
        TreesCutTotal = count;
    }
    public void SetTreesPlacedTotal(int count)
    {
        TreesPlacedTotal = count;
    }
    public void SetAnimalsKilledTotal(int count)
    {
        AnimalsKilledTotal = count;
    }
    public void SetBuildingsPlacedTotal(int count)
    {
        BuildingsPlacedTotal = count;
    }

    public int GetTreesCutTotal()
    {
        return TreesCutTotal;
    }
}
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    [Header("Chicken Settings")]
    public GameObject[] chickenPrefabs;
    public int numberOfChickens = 8;

    [Header("Spawn Bounds")]
    public Vector2 spawnAreaMin = new Vector2(-35f, -70f);
    public Vector2 spawnAreaMax = new Vector2(135f, 5f);

    [Header("Blocked Layers")]
    public LayerMask waterLayer;
    public float checkRadius = 0.2f;

    void Start()
    {
        for (int i = 0; i < numberOfChickens; i++)
        {
            SpawnChicken();
        }
    }

    void SpawnChicken()
    {
        int maxAttempts = 50;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            bool onWater = Physics2D.OverlapCircle(randomPosition, checkRadius, waterLayer);

            if (!onWater)
            {
                GameObject chosenChicken = chickenPrefabs[Random.Range(0, chickenPrefabs.Length)];
                Instantiate(chosenChicken, randomPosition, Quaternion.identity, transform);
                return;
            }
        }

        Debug.LogWarning("Could not find a valid chicken spawn position.");
    }
}
using UnityEngine;

public class CowSpawner : MonoBehaviour
{
    [Header("Cow Settings")]
    public GameObject[] cowPrefabs;
    public int numberOfCows = 5;

    [Header("Spawn Bounds")]
    public Vector2 spawnAreaMin = new Vector2(-35f, -70f);
    public Vector2 spawnAreaMax = new Vector2(135f, 5f);

    [Header("Blocked Layers")]
    public LayerMask waterLayer;
    public float checkRadius = 0.2f;

    void Start()
    {
        for (int i = 0; i < numberOfCows; i++)
        {
            SpawnCow();
        }
    }

    void SpawnCow()
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
                GameObject chosenCow = cowPrefabs[Random.Range(0, cowPrefabs.Length)];
                Instantiate(chosenCow, randomPosition, Quaternion.identity, transform);
                return;
            }
        }

        Debug.LogWarning("Could not find a valid cow spawn position.");
    }
}
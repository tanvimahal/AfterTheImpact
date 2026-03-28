using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Animal Settings")]
    [SerializeField] private GameObject[] animalPrefabs;
    [SerializeField] private float spawnInterval = 25f;
    [SerializeField] private int maxAliveAnimals = 4;
    [SerializeField] private bool startSpawningOnStart = true;

    [Header("Spawn Position")]
    [SerializeField] private bool useSpawnPoints = true;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float randomSpawnRadius = 1.5f;

    [Header("Spawn Safety")]
    [SerializeField] private bool checkCollision = true;
    [SerializeField] private float checkRadius = 0.35f;
    [SerializeField] private LayerMask blockingLayers;

    private List<GameObject> spawnedAnimals = new List<GameObject>();
    private float spawnTimer;
    private bool isSpawning;

    private void Start()
    {
        spawnTimer = spawnInterval;
        isSpawning = startSpawningOnStart;
    }

    private void Update()
    {
        if (!isSpawning || animalPrefabs == null || animalPrefabs.Length == 0)
            return;

        CleanupDestroyedAnimals();

        if (spawnedAnimals.Count >= maxAliveAnimals)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawnAnimal();
            spawnTimer = spawnInterval;
        }
    }

    private void TrySpawnAnimal()
    {
        const int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition();

            if (checkCollision)
            {
                Collider2D hit = Physics2D.OverlapCircle(spawnPosition, checkRadius, blockingLayers);
                if (hit != null)
                    continue;
            }

            GameObject chosenPrefab = animalPrefabs[Random.Range(0, animalPrefabs.Length)];

            GameObject newAnimal = Instantiate(chosenPrefab, spawnPosition, Quaternion.identity);
            spawnedAnimals.Add(newAnimal);

            // 🔑 Assign owner
            SpawnedAnimal sa = newAnimal.GetComponent<SpawnedAnimal>();
            if (sa != null)
            {
                sa.SetOwner(this);
            }

            return;
        }

        Debug.LogWarning("Spawner: Could not find valid spawn position.");
    }

    private Vector3 GetSpawnPosition()
    {
        if (useSpawnPoints && spawnPoints != null && spawnPoints.Length > 0)
        {
            Transform chosen = spawnPoints[Random.Range(0, spawnPoints.Length)];
            return chosen.position;
        }

        Vector2 offset = Random.insideUnitCircle * randomSpawnRadius;
        return transform.position + new Vector3(offset.x, offset.y, 0f);
    }

    private void CleanupDestroyedAnimals()
    {
        for (int i = spawnedAnimals.Count - 1; i >= 0; i--)
        {
            if (spawnedAnimals[i] == null)
            {
                spawnedAnimals.RemoveAt(i);
            }
        }
    }

    // 🔑 FIX FOR YOUR ERROR
    public void RemoveAnimal(GameObject animal)
    {
        if (spawnedAnimals.Contains(animal))
        {
            spawnedAnimals.Remove(animal);
        }
    }

    public void DestroyAllOwnedAnimals()
    {
        for (int i = spawnedAnimals.Count - 1; i >= 0; i--)
        {
            if (spawnedAnimals[i] != null)
            {
                Destroy(spawnedAnimals[i]);
            }
        }

        spawnedAnimals.Clear();
    }

    public void StartSpawning()
    {
        isSpawning = true;
        spawnTimer = spawnInterval;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public int GetAliveAnimalCount()
    {
        CleanupDestroyedAnimals();
        return spawnedAnimals.Count;
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.green;

    //     // Draw random spawn radius
    //     if (!useSpawnPoints)
    //     {
    //         Gizmos.DrawWireSphere(transform.position, randomSpawnRadius);
    //     }

    //     // Draw spawn points
    //     if (useSpawnPoints && spawnPoints != null)
    //     {
    //         Gizmos.color = Color.yellow;

    //         foreach (Transform point in spawnPoints)
    //         {
    //             if (point != null)
    //             {
    //                 Gizmos.DrawSphere(point.position, 0.15f);
    //             }
    //         }
    //     }
    //}
}
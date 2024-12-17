using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<WaveData> waves;         // List of wave data
    public string crystalTag = "Crystal"; // Tag of the object around which enemies will spawn
    public float spawnRadius = 5f;       // Radius of the circle around the "Crystal" to spawn enemies
    public float spawnHeight = 1f;       // Height offset for spawning (to avoid spawning underground)
    public string obstacleTag = "Obstacle"; // Tag for obstacle objects
    public List<EnemyData> enemyTypes;   // List of enemy types for this wave

    private Transform crystal;           // Reference to the "Crystal" object
    private int currentWaveIndex = 0;    // Current wave index
    private bool isSpawning = false;     // Is the wave spawner currently active?
    public bool isGameStarted = false;

    void Update()
    {
        // Only spawn waves if the game has started and no wave is currently spawning
        if (isGameStarted && !isSpawning && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        isSpawning = true;

        // Find the Crystal object just before starting the wave
        crystal = GameObject.FindGameObjectWithTag(crystalTag)?.transform;

        if (crystal == null)
        {
            Debug.LogError("Crystal object not found! Make sure the Crystal tag is assigned.");
            isSpawning = false;
            yield break; // Stop spawning if the crystal is not found
        }

        for (int i = 0; i < wave.totalEnemies; i++)
        {
            Vector3 spawnPosition;

            // Attempt to find a valid spawn position around the Crystal object
            if (FindValidSpawnPosition(out spawnPosition))
            {
                SpawnRandomEnemy(spawnPosition, enemyTypes);
            }
            else
            {
                Debug.LogWarning("No valid spawn position found for enemy!");
            }

            yield return new WaitForSeconds(wave.spawnDelay);
        }

        // Wait before the next wave
        yield return new WaitForSeconds(5f);

        currentWaveIndex++;
        isSpawning = false;
    }

    bool FindValidSpawnPosition(out Vector3 spawnPosition)
    {
        // Make sure the crystal object is assigned
        if (crystal == null)
        {
            spawnPosition = Vector3.zero;
            return false;
        }

        // Try multiple random samples to find a valid position around the "Crystal"
        for (int i = 0; i < 50; i++)
        {
            // Calculate a random point on the circle around the Crystal
            float angle = Random.Range(0f, 360f);
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * spawnRadius;

            // Calculate spawn position based on crystal's position and the random offset
            Vector3 randomPosition = crystal.position + offset;
            randomPosition.y = crystal.position.y + spawnHeight; // Adjust for height

            // Check for obstacles at this position
            if (!Physics.CheckSphere(randomPosition, spawnRadius, LayerMask.GetMask(obstacleTag)))
            {
                spawnPosition = randomPosition;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        return false;
    }

    void SpawnRandomEnemy(Vector3 position, List<EnemyData> enemyTypes)
    {
        if (enemyTypes.Count == 0) return;

        // Select a random enemy type
        EnemyData randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Count)];

        // Instantiate the enemy at the chosen position
        Instantiate(randomEnemy.enemyPrefab, position, Quaternion.identity);
    }
}

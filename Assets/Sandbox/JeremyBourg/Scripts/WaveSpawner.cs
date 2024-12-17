using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<WaveData> waves;         // List of wave data
    public Transform roomCenter;         // Center of the room
    public Vector3 roomSize;             // Size of the room
    public float spawnRadius = 0.5f;     // Radius to check for obstacles
    public string obstacleTag = "Obstacle"; // Tag for obstacle objects
    public List<EnemyData> enemyTypes;   // List of enemy types for this wave

    private int currentWaveIndex = 0;    // Current wave index
    private bool isSpawning = false;     // Is the wave spawner currently active?

    void Update()
    {
        if (!isSpawning && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        isSpawning = true;

        for (int i = 0; i < wave.totalEnemies; i++)
        {
            Vector3 spawnPosition;

            // Attempt to find a valid spawn position
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
        // Try multiple random samples to find a valid position
        for (int i = 0; i < 50; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(roomCenter.position.x - roomSize.x / 2, roomCenter.position.x + roomSize.x / 2),
                roomCenter.position.y,
                Random.Range(roomCenter.position.z - roomSize.z / 2, roomCenter.position.z + roomSize.z / 2)
            );

            // Check for obstacles
            if (!Physics.CheckSphere(randomPosition, spawnRadius, LayerMask.GetMask("Obstacle")))
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

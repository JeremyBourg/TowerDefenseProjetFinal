using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<WaveData> waves;
    public string crystalTag = "Crystal";
    public float spawnRadius = 5f;
    public float spawnHeight = 1f;
    public string obstacleTag = "Obstacle";
    public List<EnemyData> enemyTypes;

    private Transform crystal;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    public bool isGameStarted = false;

    void Update()
    {
        if (isGameStarted && !isSpawning && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        isSpawning = true;
        crystal = GameObject.FindGameObjectWithTag(crystalTag)?.transform;

        if (crystal == null)
        {
            isSpawning = false;
            yield break;
        }

        for (int i = 0; i < wave.totalEnemies; i++)
        {
            Vector3 spawnPosition;

            if (FindValidSpawnPosition(out spawnPosition))
            {
                SpawnRandomEnemy(spawnPosition, enemyTypes);
            }

            yield return new WaitForSeconds(wave.spawnDelay);
        }

        yield return new WaitForSeconds(10f);

        currentWaveIndex++;
        isSpawning = false;
    }

    bool FindValidSpawnPosition(out Vector3 spawnPosition)
    {
        if (crystal == null)
        {
            spawnPosition = Vector3.zero;
            return false;
        }

        for (int i = 0; i < 50; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * spawnRadius;
            Vector3 randomPosition = crystal.position + offset;
            randomPosition.y = crystal.position.y + spawnHeight;

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

        EnemyData randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Count)];
        Instantiate(randomEnemy.enemyPrefab, position, Quaternion.identity);
    }
}

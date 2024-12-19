using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberBehaviour : MonoBehaviour
{
    public EnemyData enemyData;
    [SerializeField] private GameObject bombPrefab;

    private GameObject[] towerArray;
    private Transform target;
    private Vector3 startPosition;
    private Vector3 direction;
    private bool hasDroppedBomb = false;
    private float timeAfterBombDropped = 0f;
    void Start()
    {
        towerArray = GameObject.FindGameObjectsWithTag("Tower");
        if (towerArray.Length == 0)
        {
            target = GameObject.FindGameObjectWithTag("Crystal").transform;
        }
        else
        {
            target = FindClosestTower();
        }

        startPosition = transform.position;

        direction = (target.position - startPosition).normalized;
        direction.y = 0f;
    }

    void Update()
    {
        BombTypeEnemyMovement();

        if (!hasDroppedBomb && IsAboveTower())
        {
            DropBomb();
        }

        if (hasDroppedBomb)
        {
            timeAfterBombDropped += Time.deltaTime;
            if (timeAfterBombDropped >= 2f)
            {
                Destroy(gameObject); 
            }
        }
    }

    Transform FindClosestTower()
    {
        Transform closestTower = towerArray[0].transform;
        float minDistance = Vector3.Distance(transform.position, closestTower.position);

        foreach (GameObject tower in towerArray)
        {
            float distance = Vector3.Distance(transform.position, tower.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTower = tower.transform;
            }
        }

        return closestTower;
    }

    void BombTypeEnemyMovement()
    {
        float step = enemyData.enemyMovementSpeed * Time.deltaTime;
        transform.position += direction * step;
    }

    bool IsAboveTower()
    {
        float tolerance = 0.05f;

        bool isAboveX = Mathf.Abs(transform.position.x - target.position.x) < tolerance;
        bool isAboveZ = Mathf.Abs(transform.position.z - target.position.z) < tolerance;

        return isAboveX && isAboveZ;
    }

    void DropBomb()
    {
        if (bombPrefab != null)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);

            hasDroppedBomb = true;
        }
    }
}

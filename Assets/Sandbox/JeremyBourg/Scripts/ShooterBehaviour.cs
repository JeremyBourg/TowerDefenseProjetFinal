using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
    public EnemyData enemyData;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject[] towerArray;
    private Transform target;
    void Start()
    {
        target = FindClosestTower();
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestTower();
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
}

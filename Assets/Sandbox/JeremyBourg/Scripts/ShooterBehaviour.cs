using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{
    public EnemyData enemyData;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform projectilePoint;

    private GameObject[] towerArray;
    private Transform target;
    private float fireTimer;
    private float orbitAngle = 0f;
    private float orbitRadius = 1f;

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
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        OrbitAroundTower();
        fireTimer += Time.deltaTime;

        if (fireTimer >= enemyData.enemyFireRate)
        {
            FireBullet();
            fireTimer = 0f;
        }
    }

    Transform FindClosestTower()
    {
        if (towerArray == null || towerArray.Length == 0) return null;

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

    void OrbitAroundTower()
    {
        if (target == null) return;

        if (enemyData.enemyMovementSpeed == 0) return;

        orbitAngle += enemyData.enemyMovementSpeed * Time.deltaTime;
        float x = target.position.x + Mathf.Cos(orbitAngle) * orbitRadius;
        float z = target.position.z + Mathf.Sin(orbitAngle) * orbitRadius;

        transform.position = new Vector3(x, transform.position.y, z);

        Vector3 directionToTarget = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget);
    }

    void FireBullet()
    {
        if (bulletPrefab == null || projectilePoint == null || target == null)
        {
            return;
        }

        Vector3 aimDirection = target.position - projectilePoint.position;
        float randomVariance = Random.Range(-5f, 5f);
        Quaternion rotationVariance = Quaternion.Euler(0, randomVariance, 0);
        Vector3 randomizedDirection = rotationVariance * aimDirection.normalized;

        GameObject bullet = Instantiate(bulletPrefab, projectilePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = randomizedDirection * 20f;
        }

        Destroy(bullet, 5f);
    }
}

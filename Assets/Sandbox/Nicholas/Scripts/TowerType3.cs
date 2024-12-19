using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerType3 : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // The projectile to shoot
    [SerializeField] private Transform shootPoint; // Where the projectile spawns
    [SerializeField] private float projectileSpeed; // Speed of the projectile
    [SerializeField] private float respawnTime; // Time before the projectile respawns if destroyed
    [SerializeField] private float shootInterval; // Time between each shot
    [SerializeField] private ScriptableTower towerData; // Reference to tower data scriptable object
    [SerializeField] private GameObject objectToDestroy; // The object to destroy when health is 0 or less

    private List<Collider> enemiesInTrigger = new List<Collider>(); // List to track enemies inside the trigger area

    void Start()
    {
        // Initialize values from the ScriptableTower
        projectileSpeed = towerData.vitesseMissile;
        respawnTime = towerData.tempsRecharge;
        shootInterval = towerData.tempsRecharge;
    }

    void Update()
    {
        // Check if the tower's health is 0 or less
        if (towerData.nbPointsVies <= 0)
        {
            Destroy(objectToDestroy); // Destroy the specified GameObject
            Debug.Log("Tower health is 0 or less. Destroying the object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object is tagged "Bullet", subtract 1 from nbPointsVies
        if (other.CompareTag("Bullet"))
        {
            towerData.nbPointsVies -= 1;
            Debug.Log("Bullet hit tower. Remaining health: " + towerData.nbPointsVies);
        }

        // Add enemy to the list when they enter the trigger
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInTrigger.Contains(other))
            {
                enemiesInTrigger.Add(other);
                StartCoroutine(ShootAtEnemies()); // Start shooting at the enemy
                Debug.Log("Enemy entered trigger: " + other.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove enemy from the list when they exit the trigger
        if (other.CompareTag("Enemy"))
        {
            enemiesInTrigger.Remove(other);
            Debug.Log("Enemy exited trigger: " + other.name);
        }
    }

    private IEnumerator ShootAtEnemies()
    {
        while (enemiesInTrigger.Count > 0)
        {
            foreach (Collider enemy in enemiesInTrigger)
            {
                ShootProjectile(enemy.transform);
            }
            yield return new WaitForSeconds(shootInterval); // Wait before shooting again
        }
    }

    private void ShootProjectile(Transform target)
    {
        Debug.Log("Shooting projectile at target: " + target.name); // Debug log to confirm projectile shooting
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Set the scale of the instantiated projectile
        projectile.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // Add the follow script dynamically
        ProjectileFollower projectileScript = projectile.AddComponent<ProjectileFollower>();
        projectileScript.SetTarget(target, projectileSpeed);
    }

    // Inner class for the projectile following logic
    public class ProjectileFollower : MonoBehaviour
    {
        private Transform target;
        private float speed;

        public Transform Target => target;

        public void SetTarget(Transform target, float speed)
        {
            this.target = target;
            this.speed = speed;
        }

        private void Update()
        {
            if (target != null)
            {
                // Move towards the target
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                // Rotate to face the target
                Vector3 direction = (target.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                Destroy(gameObject); // Destroy the projectile if the target no longer exists
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the projectile collides with the enemy
            if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject); // Destroy the projectile on collision with enemy
                Debug.Log("Projectile destroyed on collision with enemy.");
            }
        }
    }
}
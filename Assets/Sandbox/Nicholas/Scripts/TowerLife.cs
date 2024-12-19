using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TowerLife: MonoBehaviour
{
    [SerializeField] private ScriptableTower towerData;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is a "Bullet"
        if (other.CompareTag("Bullet"))
        {
            // Destroy the bullet immediately
            Destroy(other.gameObject);

            // Reduce health points of the objectToDestroy (target)
            towerData.nbPointsVies--;

            // If health is 0 or less, destroy the objectToDestroy
            if (towerData.nbPointsVies <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}


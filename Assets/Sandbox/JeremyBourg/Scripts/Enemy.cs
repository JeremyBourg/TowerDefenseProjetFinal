using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData;

 
    private void Start() {
        EnemyData.enemyHealth = EnemyData.enemyMaxHealth;
    }
    public void TakeDamage(int amount)
    {
        EnemyData.enemyHealth -= amount;
        if (EnemyData.enemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

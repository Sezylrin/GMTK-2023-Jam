using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComp : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth;
    public int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void heal(int value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void OnDeath()
    {
        if (gameObject.CompareTag(Tags.T_Enemy))
        {
            EnemyManager.instance.spawnedEnemies.Remove(gameObject);
            EnemyManager.instance.ReduceEnemy();
        }
        Destroy(gameObject);
    }
}

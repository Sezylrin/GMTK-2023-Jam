using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthComp : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth;
    public int currentHealth;
    public TMP_Text healthText;
    void Start()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
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
            currentHealth = 0;
            healthText.text = currentHealth.ToString();
            OnDeath();
        }
        else
        {
            healthText.text = currentHealth.ToString();
        }
    }

    public void heal(int value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthText.text = currentHealth.ToString();
    }

    public void OnDeath()
    {
        if (gameObject.CompareTag(Tags.T_Enemy))
        {
            EnemyManager.instance.spawnedEnemies.Remove(gameObject);
            EnemyManager.instance.ReduceEnemy();
            Instantiate(EnemyManager.instance.tokenObj, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

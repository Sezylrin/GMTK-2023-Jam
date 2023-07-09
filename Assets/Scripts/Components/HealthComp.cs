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
    public bool canTakeDamage = true;
    void Start()
    {
        SetHealth(false);
    }

    public void SetHealth(bool set, int amount = 0)
    {
        if (set)
            maxHealth = amount;
        currentHealth = maxHealth;
        healthText.text = currentHealth.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int value)
    {
        
        if (gameObject.CompareTag(Tags.T_Player))
        {
            if(canTakeDamage)
                canTakeDamage = false;
            else
            {
                Invoke("ResetTakeDamage", 1.5f);
                return;
            }    
        }
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            healthText.text = currentHealth.ToString();
            OnDeath();
        }
        else
        {
            HealthDamageText();
        }
    }

    public void ResetTakeDamage()
    {
        canTakeDamage = true;
    }

    public void heal(int value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        HealthDamageText();
    }
    public void HealthDamageText()
    {
        healthText.text = currentHealth.ToString();
    }
    [ContextMenu("die")]
    public void OnDeath()
    {
        if (gameObject.CompareTag(Tags.T_Enemy))
        {
            EnemyManager.instance.spawnedEnemies.Remove(gameObject);
            EnemyManager.instance.ReduceEnemy();
            Instantiate(EnemyManager.instance.tokenObj, transform.position + new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f)), Quaternion.identity);
        }
        if (gameObject.CompareTag(Tags.T_Player))
        {
            FindObjectOfType<SceneLoader>().LoadSceneIndex(2);
        }
        Destroy(gameObject);
    }
}

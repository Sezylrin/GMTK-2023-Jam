using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public float helpfulBuffTimer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Applyheal(bool targetPlayer)
    {
        if (targetPlayer)
        {
            HealthComp playerHealth = GameManager.instance.player.GetComponent<HealthComp>();
            playerHealth.heal((int)(playerHealth.maxHealth * 0.5f));
        }
        else
        {
            foreach (GameObject obj in EnemyManager.instance.spawnedEnemies)
            {
                HealthComp comp = obj.GetComponent<HealthComp>();
                comp.heal((int)(comp.maxHealth * 0.25f));
            }
        }
    }

    public void IncreaseDamage(bool targetPlayer)
    {
        if (targetPlayer)
        {
            
        }
    }
}

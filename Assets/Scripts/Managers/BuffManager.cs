using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public float helpfulBuffTimer;
    public bool isSwitched = false;
    public List<Weapons> weapons = new List<Weapons>();
    public DeckController deck;
    public HandController hand;
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
    public void StartDraw()
    {
        Invoke("attemptDraw", 0.5f);
    }

    public void StopDraw()
    {
        CancelInvoke("attempDraw");
    }
    public void attemptDraw()
    {
        if (GameManager.instance.currentState == gameState.fighting)
        {
            hand.PlayRandomCard();
            Invoke("attemptDraw", Random.Range(0.5f, 1f));
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastCard(Card whatCard, bool targetPlayer)
    {
        switch ((int)whatCard.info)
        {
            case ((int)CardInfo.Healing):
                Applyheal(targetPlayer);
                break;
            case ((int)CardInfo.AttackDamageUp):
                IncreaseDamage(targetPlayer);
                break;
            case ((int)CardInfo.AttackSpeedUp):
                IncreaseAttackSpeed(targetPlayer);
                break;
            case ((int)CardInfo.IncreaseMovespeed):
                IncreaseMoveSpeed(targetPlayer);
                break;
            case ((int)CardInfo.SwitchDamageAndHealth):
                SwitchHealthDamage();
                break;
            case ((int)CardInfo.Sword):
                SwitchWeaponSword();
                break;
            case ((int)CardInfo.Spear):
                SwitchWeaponSpear();
                break;
            case ((int)CardInfo.Dagger):
                SwitchWeaponDagger();
                break;
            case ((int)CardInfo.Fireball):
                break;
            case ((int)CardInfo.Iceball):
                break;
            case ((int)CardInfo.MeteorShower):
                break;




        }
    }

    public void Applyheal(bool targetPlayer)
    {
        if (targetPlayer)
        {
            HealthComp playerHealth = GameManager.instance.playerHealth;
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
            GameManager.instance.playerInfo.AddDamage(2);
        }
        else
        {
            foreach (GameObject obj in EnemyManager.instance.spawnedEnemies)
            {
                EnemyAI comp = obj.GetComponent<EnemyAI>();
                comp.damage++;
            }
        }
    }

    public void IncreaseAttackSpeed(bool targetPlayer)
    {
        if (targetPlayer)
        {
            GameManager.instance.playerInfo.IncreaseAttackSpeed();
        }
        else
        {
            foreach (GameObject obj in EnemyManager.instance.spawnedEnemies)
            {
                EnemyAI comp = obj.GetComponent<EnemyAI>();
                comp.IncreaseSpeed();
            }
        }
    }

    public void IncreaseMoveSpeed(bool targetPlayer)
    {
        if (targetPlayer)
        {
            GameManager.instance.playerInfo.IncreaseSpeed(2.5f);
        }
        else
        {
            foreach (GameObject obj in EnemyManager.instance.spawnedEnemies)
            {
                EnemyAI comp = obj.GetComponent<EnemyAI>();
                comp.IncreaseMoveSpeed(2.5f);
            }
        }
    }

    public void SwitchHealthDamage()
    {
        int currentDamage = GameManager.instance.playerInfo.currentDamage;
        GameManager.instance.playerInfo.currentDamage = GameManager.instance.playerHealth.currentHealth;
        GameManager.instance.playerHealth.currentHealth = currentDamage;
        isSwitched = true;
    }
    [ContextMenu("SwitchWeaponSword")]
    public void SwitchWeaponSword()
    {
        GameManager.instance.playerInfo.ChangeWeapon(weapons[0]);
    }
    [ContextMenu("SwitchWeaponSpear")]
    public void SwitchWeaponSpear()
    {
        GameManager.instance.playerInfo.ChangeWeapon(weapons[1]);
    }

    [ContextMenu("SwitchWeaponDagger")]
    public void SwitchWeaponDagger()
    {
        GameManager.instance.playerInfo.ChangeWeapon(weapons[2]);
    }
}

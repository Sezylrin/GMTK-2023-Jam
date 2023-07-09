using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum gameState
{
    starting,
    drawing,
    shop,
    fighting,
    enemyHealth
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public HealthComp playerHealth;
    public PlayerController playerInfo;
    public int enemyHealth;
    public gameState currentState;
    public int tokens;
    public int startingMana;
    public int maxMana;
    public int currentMana;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        if (!player)
        {
            player = GameObject.FindWithTag(Tags.T_Player);
        }
        if (!playerHealth)
        {
            playerHealth = player.GetComponent<HealthComp>();
        }
        if (!playerInfo)
        {
            playerInfo = player.GetComponent<PlayerController>();
        }

        maxMana = startingMana;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool UseToken(int amount)
    {
        if (tokens - amount < 0)
            return false;
        else
        {
            tokens -= amount;
            GameUI.instance.TokenText();
            return true;
        }
    }
    public void lowerEnemyHealth(int amount)
    {
        enemyHealth -= amount;

        if (enemyHealth <= 0)
        {
            FindObjectOfType<SceneLoader>().LoadSceneIndex(3);
        }
    }

    public void SetMana()
    {
        currentMana = maxMana;
    }

    public void UseMana(int amount)
    {
        currentMana -= amount;
        GameUI.instance.ManaText();
    }
}

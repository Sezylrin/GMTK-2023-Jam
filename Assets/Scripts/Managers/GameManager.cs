using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum gameState
{
    starting,
    drawing,
    shop,
    fighting
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public int enemyHealth;
    public gameState currentState;
    public int tokens;
    public int startingMana;
    public int maxMana;
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
        if (!player)
        {
            player = GameObject.FindWithTag(Tags.T_Player);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        maxMana = startingMana;
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
            return true;
        }
    }
    public void lowerEnemyHealth(int amount)
    {
        enemyHealth -= amount;

        if (enemyHealth <= 0)
        {
            //win
        }
    }
}

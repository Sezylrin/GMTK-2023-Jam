using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    // Start is called before the first frame update
    public List<GameObject> enemyTypes = new List<GameObject>();
    public List<GameObject> spawnedEnemies;
    public List<Weapons> Weapons;
    public Transform spawnCentre;
    public GameObject tokenObj;
    public float separationDist;
    private int enemiesSpawned;
    public GameObject HeartObj;


    public int spawnAmount;
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
    [ContextMenu("spawnenemy")]
    public void spawnTest()
    {
        SpawnEnemy(spawnAmount);
    }

    public void SpawnEnemy(int amount)
    {
        enemiesSpawned = amount;
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], spawnCentre.position, Quaternion.identity);
            enemy.GetComponent<EnemyAI>().ChangeWeapon(Weapons[Random.Range(0, Weapons.Count)]);
            Vector3 pos = enemy.transform.position;
            pos.x = spawnCentre.position.x - (((float)amount - 1) * 0.5f * separationDist) + (i * separationDist);
            enemy.transform.position = pos;
            spawnedEnemies.Add(enemy);
        }
    }

    public void ReduceEnemy()
    {
        enemiesSpawned--;
        if (enemiesSpawned == 0)
        {
            SpawnHeart();
        }
    }

    public void SpawnHeart()
    {
        GameObject heart = Instantiate(HeartObj, Vector2.up * 2.5f, Quaternion.identity);
        heart.GetComponentInChildren<TMP_Text>().text = GameManager.instance.enemyHealth.ToString();
    }
    public void SpawnShop()
    {
        GameManager.instance.currentState = gameState.shop;
        GameUI.instance.ResetUI();
        GameManager.instance.playerInfo.ResetStats();
    }
    
}

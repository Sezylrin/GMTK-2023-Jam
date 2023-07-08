using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    // Start is called before the first frame update
    public List<GameObject> enemyTypes = new List<GameObject>();
    public List<GameObject> spawnedEnemies;
    public Transform spawnCentre;
    public float separationDist;
    private int enemiesSpawned;

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
            //transtion to token stage
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject startBattleBtn;
    // Start is called before the first frame update
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

    public void StartRound()
    {
        EnemyManager.instance.SpawnEnemy(Random.Range(1, 6));
        GameManager.instance.currentState = gameState.fighting;
        startBattleBtn.SetActive(false);
    }

    public void Reset()
    {
        startBattleBtn.SetActive(true);
    }
}

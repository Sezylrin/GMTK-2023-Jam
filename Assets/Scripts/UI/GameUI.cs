using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject startBattleBtn;
    public GameObject ShopObj;
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
        GameManager.instance.SetMana();
        startBattleBtn.SetActive(false);
        ShopObj.SetActive(false);
        BuffManager.instance.StartDraw();
    }

    public void ResetUI()
    {
        startBattleBtn.SetActive(true);
        ShopObj.SetActive(true);
    }
}

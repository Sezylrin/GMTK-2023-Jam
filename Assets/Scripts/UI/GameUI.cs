using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject startBattleBtn;
    public GameObject ShopObj;

    public TMP_Text manaText;
    public TMP_Text tokenText;
    public bool isStart = true;
    // Start is called before the first frame update
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
    }
    void Start()
    {
        ManaText();
        TokenText();
    }

    public void ManaText()
    {
        manaText.text = GameManager.instance.currentMana.ToString() + "/" + GameManager.instance.maxMana.ToString();
    }

    public void TokenText()
    {
        tokenText.text = "x" + GameManager.instance.tokens.ToString();

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRound()
    {
        switch (4 - Mathf.CeilToInt((float)GameManager.instance.enemyHealth / 10f))
        {
            case (1):
                EnemyManager.instance.SpawnEnemy(Random.Range(2, 4));
                break;
            case (2):
                EnemyManager.instance.SpawnEnemy(Random.Range(3, 5));
                break;
            case (3):
                EnemyManager.instance.SpawnEnemy(Random.Range(4, 6));
                break; ;
        }
        GameManager.instance.currentState = gameState.fighting;
        startBattleBtn.SetActive(false);
        ShopObj.SetActive(false);
        BuffManager.instance.StartDraw();
        BuffManager.instance.StartEnemyDraw();
    }

    public void ResetUI()
    {
        startBattleBtn.SetActive(true);
        if(!isStart)
        {
            ShopObj.SetActive(true);
        }
        else
        {
            isStart = false;
        }
        GameManager.instance.SetMana();
        ManaText();
    }
}
